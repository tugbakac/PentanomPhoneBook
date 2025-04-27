Bilgisayarınız Postgres kurulu olması gerekmektedir.
Proje .Net 8.0 ile yazılmış olup yüklü değilse bilgisayarınıza kurmanız daha sağlıklı sonuç almanızı sağlar.
Solution içerisinde iki servis projesi bulunmaktadır.ContactService, ReportService projelerin içerisindeki appsettings.json
dosyasında veritabanı bilgilerinizi güncellemeniz gerekir.
Kafka ile ilgili bilgilerde aynı şekilde appsettings.json üzerinden kontrol edilmelidir.
ContactService.Test projesini Test Explorer üzerinden çalıştırıp ContactService projesinin test işlemini gerçekleştirebilirsiniz.
ReportService.Test projesini Test Explorer üzerinden çalıştırıp ReportService projesinin test işlemini gerçekleştirebilirsiniz.
