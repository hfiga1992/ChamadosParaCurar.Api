using ChamadosParaCurar.Api.Models;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;
using System.Collections.Generic;

namespace ChamadosParaCurar.Api.Services
{
    public class CertificadoService
    {
        public byte[] GerarCertificado(Usuario usuario)
        {
            // Configurações
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");
            
            // Cores
            BaseColor corPreta = new BaseColor(40, 40, 40);
            BaseColor corDourada = new BaseColor(212, 175, 55);
            BaseColor corBranca = new BaseColor(255, 255, 255); // Branco
            
            using (var ms = new MemoryStream())
            {
                // Configurar o documento PDF
                var documento = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);
                var writer = PdfWriter.GetInstance(documento, ms);
                documento.Open();
                
                // Definir fundo preto para a página
                PdfContentByte cb = writer.DirectContent;
                Rectangle pageSize = documento.PageSize;
                cb.SetColorFill(corPreta);
                cb.Rectangle(0, 0, pageSize.Width, pageSize.Height);
                cb.Fill();
                
                // Adicionar bordas decorativas douradas
                // Canto superior esquerdo
                cb.SetColorFill(corDourada);
                cb.MoveTo(0, pageSize.Height);
                cb.LineTo(100, pageSize.Height);
                cb.LineTo(0, pageSize.Height - 100);
                cb.Fill();
                
                // Canto superior direito
                cb.MoveTo(pageSize.Width, pageSize.Height);
                cb.LineTo(pageSize.Width - 100, pageSize.Height);
                cb.LineTo(pageSize.Width, pageSize.Height - 100);
                cb.Fill();
                
                // Canto inferior esquerdo
                cb.MoveTo(0, 0);
                cb.LineTo(100, 0);
                cb.LineTo(0, 100);
                cb.Fill();
                
                // Canto inferior direito
                cb.MoveTo(pageSize.Width, 0);
                cb.LineTo(pageSize.Width - 100, 0);
                cb.LineTo(pageSize.Width, 100);
                cb.Fill();
                
                // Borda dourada retangular
                cb.SetColorStroke(corDourada);
                cb.SetLineWidth(2);
                float margin = 40;
                cb.Rectangle(margin, margin, pageSize.Width - 2 * margin, pageSize.Height - 2 * margin);
                cb.Stroke();
                
                // Definir fontes
                var fonteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 28, corBranca);
                var fonteSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, corBranca);
                var fonteMensagem = FontFactory.GetFont(FontFactory.HELVETICA, 14, corDourada);
                var fonteNome = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 36, corBranca);
                var fonteTexto = FontFactory.GetFont(FontFactory.HELVETICA, 12, corDourada);
                var fonteAssinatura = FontFactory.GetFont(FontFactory.HELVETICA, 10, corDourada);
                
                // Espaço superior
                documento.Add(new Paragraph(" ", fonteTitulo));
                documento.Add(new Paragraph(" ", fonteTitulo));
                
                // Título
                var titulo = new Paragraph("CERTIFICADO", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo);
                
                // Subtítulo
                var subtitulo = new Paragraph("- DE CONQUISTA -", fonteSubtitulo);
                subtitulo.Alignment = Element.ALIGN_CENTER;
                subtitulo.SpacingAfter = 30;
                documento.Add(subtitulo);
                
                // Mensagem
                var mensagem = new Paragraph("ESTE DIPLOMA PERTENCE A", fonteMensagem);
                mensagem.Alignment = Element.ALIGN_CENTER;
                mensagem.SpacingAfter = 20;
                documento.Add(mensagem);
                
                // Nome do usuário
                var nomeUsuario = new Paragraph(usuario.Nome, fonteNome);
                nomeUsuario.Alignment = Element.ALIGN_CENTER;
                nomeUsuario.SpacingAfter = 20;
                documento.Add(nomeUsuario);
                
                // Informação do curso
                var infoCurso = new Paragraph("por concluir o curso Chamados para Curar", fonteTexto);
                infoCurso.Alignment = Element.ALIGN_CENTER;
                infoCurso.SpacingAfter = 50;
                documento.Add(infoCurso);
                
                // Imagem medalha (simulado com um círculo)
                float centerX = pageSize.Width / 2;
                float centerY = pageSize.Height / 2 - 80;
                cb.SetColorFill(corDourada);
                cb.Circle(centerX, centerY, 20);
                cb.Fill();
                BaseColor corDouradaEscura = new BaseColor(172, 135, 15);
                cb.SetColorStroke(corDouradaEscura);
                cb.Circle(centerX, centerY, 23);
                cb.Stroke();
                
                // Assinaturas
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 80;
                table.SetWidths(new float[] { 1, 1, 1 });
                
                // Linha assinatura 1
                PdfPCell cell1 = new PdfPCell();
                cell1.Border = PdfPCell.TOP_BORDER;
                cell1.BorderColor = corDourada;
                cell1.PaddingTop = 5;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                Paragraph p1 = new Paragraph("NOME", fonteAssinatura);
                p1.Alignment = Element.ALIGN_CENTER;
                cell1.AddElement(p1);
                
                Paragraph p1Sub = new Paragraph("Recebedor do certificado", fonteAssinatura);
                p1Sub.Alignment = Element.ALIGN_CENTER;
                cell1.AddElement(p1Sub);
                
                table.AddCell(cell1);
                
                // Célula vazia do meio
                PdfPCell cellMid = new PdfPCell();
                cellMid.Border = PdfPCell.NO_BORDER;
                cellMid.FixedHeight = 50;
                table.AddCell(cellMid);
                
                // Linha assinatura 2
                PdfPCell cell2 = new PdfPCell();
                cell2.Border = PdfPCell.TOP_BORDER;
                cell2.BorderColor = corDourada;
                cell2.PaddingTop = 5;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                Paragraph p2 = new Paragraph("NOME", fonteAssinatura);
                p2.Alignment = Element.ALIGN_CENTER;
                cell2.AddElement(p2);
                
                Paragraph p2Sub = new Paragraph("Diretor responsável", fonteAssinatura);
                p2Sub.Alignment = Element.ALIGN_CENTER;
                cell2.AddElement(p2Sub);
                
                table.AddCell(cell2);
                
                documento.Add(table);
                
                // Data de emissão
                var dataEmissao = new Paragraph($"Emitido em {dataAtual}", fonteTexto);
                dataEmissao.Alignment = Element.ALIGN_CENTER;
                dataEmissao.SpacingBefore = 40;
                documento.Add(dataEmissao);
                
                // Fechar o documento
                documento.Close();
                writer.Close();
                
                // Retornar o PDF como array de bytes
                return ms.ToArray();
            }
        }
    }
}
