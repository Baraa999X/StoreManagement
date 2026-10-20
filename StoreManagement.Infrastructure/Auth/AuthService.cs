using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Application.Auth;
using StoreManagement.Application.Contracts;
using StoreManagement.Domain.Entities;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManagement.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICustomerData _customerData;
        private readonly StoreDbContext _context;
        public AuthService(UserManager<ApplicationUser> userManager,IConfiguration configuration, ICustomerData customerData, StoreDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _customerData = customerData;
            _context = context;
        }



        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            // منبدأ Transaction حتى نجمع عمليات التسجيل ضمن معاملة واحدة.
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // أول شي منجهّز حساب المستخدم.
                var user = new ApplicationUser  // في اي دي هون بينشئ تلقائي
                {
                    UserName = dto.Email,
                    Email = dto.Email
                };

                // Identity بتنشىء الحساب وبتتحقق من شروط كلمة المرور.
                var result = await _userManager.CreateAsync(user, dto.Password);// هون نحفظ بقاعده البيانات ال id

                // إذا فشل إنشاء الحساب، منتراجع عن الـ Transaction.
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // منضيف المستخدم لدور Customer.
                var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

                // إذا فشلت إضافة الدور، منتراجع عن عمليات التسجيل.
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // منجهّز سجل العميل بجدول Customers.
                var customer = new Customer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                // منحفظ سجل العميل ومنربطه بحساب Identity عن طريق user.Id.
                _customerData.AddCustomerForUser(customer, user.Id);  // هون ستعملناه

                // مننشئ JWT Token للمستخدم بعد نجاح عمليات التسجيل.
                var token = await GenerateTokenAsync(user);

                // منثبّت تغييرات قاعدة البيانات بعد نجاح العمليات.
                await transaction.CommitAsync();

                return new AuthResponseDto
                {
                    Token = token
                };
            }
            catch
            {
                // إذا صار استثناء غير متوقع، منحاول نتراجع عن التغييرات.
                await transaction.RollbackAsync();

                // منعيد رمي الاستثناء حتى ما نخفي الخطأ الحقيقي.
                throw;
            }
        }



        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
            {
                return null;
            }

            var token = await GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Token = token
            };
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>//كلاس اسمو هيك شغلتوا للسيكورتي
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));//هاد بيقول إن المعلومة هي دور المستخدم، وقيمتها مثلاً Customer أو Admin
            }

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT signing key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials( key,SecurityAlgorithms.HmacSha256);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var token = new JwtSecurityToken(
             issuer: issuer,
             audience: audience,
             claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}