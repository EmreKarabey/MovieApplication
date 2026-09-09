using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.UpdateEmail;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Application.Features.Login.Command.SenderCode
{
    public class SenderCodeCommand : IRequest, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }
        public string? CacheKey => $"SenderCodeCommand Email:{Email}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class SenderCodeCommandHandler : IRequestHandler<SenderCodeCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        public SenderCodeCommandHandler(IUserRepository userRepository, IMapper mapper, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task Handle(SenderCodeCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user == null) throw new BusinessException("User Not Found");

            Random rnd = new Random();

            int code = rnd.Next(100000, 999999);

            DateTime codeDuration = DateTime.UtcNow.AddMinutes(5);

            user.CodeDuration = codeDuration;
            user.Code = code;

            await _userRepository.UpdateAsync(user);

            string mailBody = $@"
<div style='font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; background-color: #f4f5f7; padding: 40px 20px; text-align: center;'>
    <div style='max-width: 500px; margin: 0 auto; background-color: #ffffff; padding: 35px; border-radius: 16px; box-shadow: 0 4px 15px rgba(0,0,0,0.05); text-align: left;'>
        
        <!-- Logo veya Başlık Alanı -->
        <div style='text-align: center; margin-bottom: 30px;'>
            <h2 style='color: #4F46E5; margin: 0; font-size: 26px; font-weight: 800; letter-spacing: -0.5px;'>MovieApp</h2>
            <p style='color: #6B7280; margin: 5px 0 0 0; font-size: 14px; font-weight: 500;'>Hesap Güvenliği Doğrulaması</p>
        </div>
        
        <!-- Giriş Metni -->
        <p style='color: #1F2937; font-size: 16px; line-height: 1.6; margin: 0 0 16px 0;'>Merhaba,</p>
        <p style='color: #4B5563; font-size: 15px; line-height: 1.6; margin: 0 0 24px 0;'>Hesabınıza ait e-posta adresini değiştirmek için bir talepte bulundunuz. İşleminize güvenli bir şekilde devam edebilmeniz için tek kullanımlık doğrulama kodunuz aşağıdadır:</p>
        
        <!-- Kod Alanı (Renkli ve Vurgulu) -->
        <div style='background-color: #EEF2F6; border: 2px dashed #4F46E5; border-radius: 12px; padding: 20px; text-align: center; margin-bottom: 24px;'>
            <span style='font-size: 36px; font-weight: 800; letter-spacing: 8px; color: #1F2937; display: inline-block; padding-left: 8px;'>{code}</span>
        </div>
        
        <!-- Süre Uyarısı (Amber/Uyarı Rengi) -->
        <div style='background-color: #FFFBEB; border-left: 4px solid #F59E0B; padding: 14px; margin-bottom: 28px; border-radius: 4px 12px 12px 4px;'>
            <p style='color: #B45309; margin: 0; font-size: 13.5px; line-height: 1.5; font-weight: 500;'>
                ⏱️ <b>Süre Sınırı:</b> Bu kod güvenlik nedeniyle sadece <b>5 dakika</b> geçerlidir. Süre dolduğunda uygulamadan yeni bir kod talep etmeniz gerekir.
            </p>
        </div>
        
        <!-- Ayırıcı Çizgi -->
        <hr style='border: none; border-top: 1px solid #E5E7EB; margin-bottom: 20px;' />
        
        <!-- Bilgilendirme ve Kapanış -->
        <p style='color: #9CA3AF; font-size: 12.5px; line-height: 1.5; margin: 0 0 20px 0;'>
            Eğer bu işlemi siz başlatmadıysanız, lütfen bu e-postayı dikkate almayınız. Hesabınız şu an güvendedir.
        </p>
        
        <p style='color: #4F46E5; font-size: 14px; font-weight: 700; margin: 0; text-align: center;'>
            Keyifli günler dileriz!<br/>
            <span style='color: #9CA3AF; font-weight: 500; font-size: 12px;'>MovieApp Ekibi</span>
        </p>
        
    </div>
</div>
";

            await _emailSender.SendEmailAsync(request.Email, "Onay Kodu", mailBody);
        }
    }
}
