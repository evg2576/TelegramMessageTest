using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using TL;
using WTelegram;

int apiId = 23690756;
string apiHash = "3c162901b8bf0bb5e53124d27f8cfdce";
string to = "jesterq";

//using var client = new Client(apiId, apiHash);
//await client.ConnectAsync();
//client.CollectAccessHash = true;
//var resQr = (Auth_LoginToken)await client.Auth_ExportLoginToken(apiId, apiHash);
//string qr = Convert.ToBase64String(resQr.token);

//QRCodeGenerator qrGenerator = new QRCodeGenerator();
//QRCodeData qrCodeData = qrGenerator.CreateQrCode($"tg://login?token={qr}", QRCodeGenerator.ECCLevel.Q);
//QRCode qrCode = new QRCode(qrCodeData);
//Bitmap qrCodeImage = qrCode.GetGraphic(20);
//qrCodeImage.Save("c:\\TgQR.png", ImageFormat.Png);
//Contacts_ResolvedPeer res;

Client client = new Client(apiId, apiHash);
await DoLogin("+79286072587"); // initial call with user's phone_number

async Task DoLogin(string loginInfo) // (add this method to your code)
{
    while (client.User == null)
        switch (await client.Login(loginInfo)) // returns which config is needed to continue login
        {
            case "verification_code": Console.Write("Code: "); loginInfo = ""; break;
            case "name": loginInfo = "John Doe"; break;    // if sign-up is required (first/last_name)
            case "password": loginInfo = "secret!"; break; // if user has enabled 2FA
            default: loginInfo = null; break;
        }
    Console.WriteLine($"We are logged-in as {client.User} (id {client.User.id})");
}

Contacts_ResolvedPeer res;
if (to.Any(c => char.IsLetter(c)))
    res = await client.Contacts_ResolveUsername(to);
else res = await client.Contacts_ResolvePhone(to);
await client.SendMessageAsync(res.User, "123321");
