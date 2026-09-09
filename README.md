# Movie Application 🎬

Modern .NET teknolojileri ile geliştirilmiş kapsamlı, kurumsal seviyede bir Film Yönetimi ve Topluluk Platformu. Bu proje, **Clean Architecture (Temiz Mimari)** prensiplerinin, **CQRS** tasarım deseninin ve **Yapay Zeka (AI) destekli özelliklerin** nasıl uygulanacağını göstermektedir.

## 🚀 Özellikler

- **Sağlam Mimari:** Clean Architecture (Domain, Application, Infrastructure, Persistence, UI/API katmanları) kullanılarak geliştirilmiş, bağımlılıkların ayrılması ve test edilebilirlik sağlanmıştır.
- **CQRS Tasarım Deseni:** İstek yönetimini ölçeklenebilir ve sürdürülebilir kılmak için Komutlar (Commands) ve Sorgular (Queries) MediatR aracılığıyla ayrılmıştır.
- **Kimlik Doğrulama & Yetkilendirme (Auth):** 
  - JWT (JSON Web Tokens) tabanlı kimlik doğrulama.
  - Google OAuth (Google ile giriş) entegrasyonu.
- **Topluluk Özellikleri:**
  - Forum sistemi ve tartışma kategorileri.
  - İç içe (alt yorumlar) destekli film yorum sistemi.
  - Kullanıcı aktivite takibi (Geçmiş, Beğeniler, Beğenmemeler, Favoriler, Kaydedilenler).
  - Abonelik (Subscription) sistemi.
- **Yapay Zeka (AI) Entegrasyonları (HuggingFace):**
  - **ToxicBert:** Topluluk kurallarını korumak için yorumlarda ve forum gönderilerinde otomatik toksisite/küfür tespiti.
  - **Helsinki-NLP:** Otomatik çeviri hizmetleri entegrasyonu.
- **Gelişmiş Loglama:** **Serilog** kullanılarak merkezi loglama sistemi kurulmuş, logların hem dosyalara hem de MS SQL Server'a otomatik yazılması sağlanmıştır.
- **Anlık Bildirimler:** Gerçek zamanlı bildirimler için kullanıcı FCM (Firebase Cloud Messaging) token yönetimi.
- **E-posta Hizmetleri:** E-posta bildirimleri için SMTP entegrasyonu.

## 💻 Kullanılan Teknolojiler (Tech Stack)

### Backend (Sunucu Tarafı)
- **Framework:** .NET 9 (ASP.NET Core Web API)
- **Mimari:** Clean Architecture, CQRS
- **Veritabanı ORM:** Entity Framework Core (Code First)
- **Mediator Deseni:** MediatR
- **Loglama:** Serilog
- **Kimlik Doğrulama:** JWT, Google Authentication
- **Yapay Zeka/Makine Öğrenmesi:** HuggingFace Inference API

### Frontend (Kullanıcı Arayüzü)
- **Framework:** ASP.NET Core MVC
- **Arayüz (UI):** HTML, CSS, JavaScript (Bootstrap/Özel Tasarım)

## 📁 Proje Yapısı

Çözüm (Solution), sıkı bir Clean Architecture modelini takip eder:

*   **`Domain`**: Kurumsal düzeydeki mantığı ve tipleri (Varlıklar, Enum'lar, Değer Nesneleri) içerir. Başka hiçbir projeye bağımlı değildir.
*   **`Application`**: İş mantığını, CQRS İşleyicilerini (Commands/Queries), DTO'ları ve arayüzleri içerir. Sadece Domain katmanına bağımlıdır.
*   **`Infrastructure`**: Dış sistemlerin (E-posta gönderimi, AI entegrasyonları, Token oluşturma, Identity) uygulandığı katmandır.
*   **`Persistence`**: Veri erişim mantığının (Entity Framework Core DbContext, Repository sınıfları, Migration'lar) uygulandığı katmandır.
*   **`MovieApplication` (Web API)**: Backend uygulamasının giriş noktasıdır. HTTP isteklerini, bağımlılık enjeksiyonunu (Dependency Injection) ve yapılandırmaları yönetir.
*   **`MovieApplicationUI` (MVC)**: Backend API'si ile iletişim kuran, kullanıcıların etkileşime girdiği önyüz (frontend) uygulamasıdır.

## 🛠️ Başlangıç Kılavuzu

### Gereksinimler
- [.NET 9 SDK](https://dotnet.microsoft.com/download) (veya daha güncel bir sürüm)
- SQL Server (LocalDB veya tam sürüm)
- Visual Studio 2022 / JetBrains Rider / VS Code

### Kurulum

1. **Depoyu bilgisayarınıza klonlayın:**
   ```bash
   git clone https://github.com/EmreKarabey/MovieApplication.git
   cd MovieApplication
   ```

2. **Uygulama Ayarlarını (AppSettings) Yapılandırın:**
   API projesine (`MovieApplication/MovieApplication/appsettings.json`) gidin ve gerekli alanları doldurun:
   - `ConnectionStrings:DefaultConnection`: LocalDB kullanmıyorsanız veritabanı bağlantı cümlenizi güncelleyin.
   - `TokenOptions`: JWT `SecurityKey` bilginizi girin.
   - `Smtp`: Kendi SMTP giriş bilgilerinizi ekleyin.
   - `ToxicBert` & `Helsinki`: Kendi HuggingFace Token'ınızı girin.
   - `Google`: Kendi Google Client ID ve Secret bilgilerinizi girin.

3. **Veritabanını Oluşturun (Migrations):**
   Visual Studio'da Package Manager Console'u açın, Default project olarak `Persistence`ı seçin ve çalıştırın:
   ```powershell
   Update-Database
   ```
   Veya projenin ana dizininde .NET CLI kullanarak:
   ```bash
   dotnet ef database update --project MovieApplication\Persistence\Persistence.csproj --startup-project MovieApplication\MovieApplication\MovieApplication.csproj
   ```

4. **Uygulamayı Çalıştırın:**
   `MovieApplication` (API) ve `MovieApplicationUI` (MVC) projelerinin her ikisini de başlangıç projesi (Startup Project) olarak ayarlayın veya ayrı ayrı `dotnet run` komutuyla çalıştırın.

## 🛡️ Güvenlik Notu
Bu projede, hassas API anahtarlarının, veritabanı bağlantılarının veya SMTP şifrelerinin Github deposuna sızmasını engellemek amacıyla `.gitignore` ve `appsettings.json` içerisinde yer tutucular (placeholders) kullanılmıştır. 

## 👨‍💻 Geliştirici
**Emre Karabey**
- [GitHub](https://github.com/EmreKarabey)
