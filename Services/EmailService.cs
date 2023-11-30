using APIEnviaEmail.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
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

    public async Task<bool> EnviarEmailAsync(string destinatario, Email email, Stream file)
    {
        try
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

                    mensagem.Subject = email.Assunto;
                    mensagem.Body = email.Corpo;
                    mensagem.Attachments.Add(new Attachment(file, "holerite.pdf"));

                    await clienteSmtp.SendMailAsync(mensagem);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
        
    }

    public Email EscreveEmail(string nomeFuncionario, string tipoHolerite, string mes)
    {
        var email = new Email();

        email.Corpo = $"Olá, {nomeFuncionario}\nSegue o seu recibo de {tipoHolerite} do mês de {mes} em anexo.\nAbraços,\nRH";
        email.Assunto = $"Recibo de {tipoHolerite} do mês de {mes}";

        return email;
    }
}
