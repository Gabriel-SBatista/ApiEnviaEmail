using System.IO.Compression;
using System.Net;
using System.Net.Mail;

namespace APIEnviaEmail.Services;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
    }

    public async Task EnviarEmailAsync(string destinatario, string assunto, string corpo, Stream file)
    {
        using (SmtpClient clienteSmtp = new SmtpClient(_smtpServer))
        {
            clienteSmtp.Port = _smtpPort;
            clienteSmtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            clienteSmtp.EnableSsl = true;

            using (MailMessage mensagem = new MailMessage())
            {
                mensagem.From = new MailAddress(_smtpUsername);

                mensagem.To.Add(destinatario);

                mensagem.Subject = assunto;
                mensagem.Body = corpo;
                mensagem.Attachments.Add(new Attachment(file, "holerite.pdf"));

                await clienteSmtp.SendMailAsync(mensagem);
            }
        }
    }
}
