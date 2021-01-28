# SmartBookCase
4.Sınıf 1.Dönem Yazılım Bakımı Dersi Projesi, Kütüphane Otomasyonu Web Uygulaması

Bu program, Kütüphane çalışanlarının kullanacağı varsayılarak hazırlanmıştır.

Kütüphane çalışanları hesap oluşturur ve sistemi kullanır, sistemde kitap ve üye ekleme-güncelleme-silme işlemlerini gerçekleştirebilirler.

kiralama işlemlerini gerçekleştirebilirler ve kiralama işlemlerinde kütüphane görevilsinin bilgiside saklanmaktadır.

Kiralama işlemlerinden özel olarak bir kitabın kiralamalarına girilip kitabın kimlere verilmiş olduğu görülebilir.

Bir kullanıcı geçmişten bugüne hangi kitapları almış, teslim durumları nedir? Tarihleri ile herşey sistem tarafından saklanır.

Kiralama işlemleri kütüphane üyelerinin tc no'su ile yapılmaktadır ve Her operasyonda üyenin mailine bilgi gitmektedir, kitabı ne zaman teslim etmesi gerektiği vs.

Kiralama işlemleri ile ilgili sürede uzatma yapılabilir.

Arayüzlerde farklı renkler kullanarak stok durumu kritik olanlar - kalmayanlar tablolarda renklendirilmiştir.

Kitap ve üye silme işlemlerinde güvenlik amaçlı Kütüphane görevlisine şifresi sorulur, bunun yanında silinecek üye veya kitap için kayıtlı bir kiralama var ise silme işlemi gerçekleşmez.

Kütüphane kullanıcıları şifrelerini unuttuğunda sistemden yeni şifre alabilirler, yeni şifre görevlinin mailine gönderilir.

Kütüphane görevlisi şifresini 3 kez üst üste yanlış girer ise, Şifre Bloke edilir ve sistemden yeni şifre alması gerekir.

Password-phone validation mevcuttur, şifrenin gücünü gösteren strength-bar gerekli yerlerde kullanılmıştır.

Şifre MD5 algoritması ile şifrelenip saklanmaktadır.

Yeni Kitap kayıtlarında bir yanlışlık olmaması için isim ve barkod veritabanından kontrol edilir, aynı şekilde yeni üyelerin kaydında e-mail ve tc'no veritabanı kontrol edilir. Eğer herhangi biri aynı olur ise kayıt gerçekleşmez kütüphane görevlisine gerekli bilgi verilir.

Sistemde karşılaşılabilecek her hata için bilgilendirme mesajı gösterilir. Şifre hatalı, Böyle bir mail yok, tc'no hatalı, bu mail zaten kayıtlı...........
