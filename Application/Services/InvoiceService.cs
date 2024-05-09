using Application.DataAccess;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Application.Utils;
using PdfSharp.Fonts;

namespace Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConsumerService _consumerService;
        private readonly IHouseService _houseService;

        public InvoiceService(IInvoiceRepository invoiceRepository, IConsumerService consumerService, IHouseService houseService, ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _consumerService = consumerService;
            _houseService = houseService;
            _logger = logger;
        }

        public async Task<Receipt> GetInvoiceById(int id)
        {
            return await _invoiceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Receipt>> GetInvoiceByPersonalAccount(string personalAccount)
        {
            return await _invoiceRepository.GetByPersonalAccountAsync(personalAccount);
        }

        public async Task<List<Receipt>> GetList()
        {
            try
            {
                var all = await _invoiceRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} invoices from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving invoices from the database.");
                throw;
            }
        }

        public async Task<Receipt> Add(params object[] data)
        {
            var dataList = (List<(string, List<(string, int, int)>)>)data[0];
            var personalAccount = dataList[0].Item1;
            var serviseInfo = dataList[0].Item2;
            var consumer = await _consumerService.GetConsumerByPersonalAccount(personalAccount);
            var hause = await _houseService.GetHouseById(consumer.HouseId);
            var pdf = GeneratePdf(personalAccount, hause.HouseAddress, consumer.Flat, consumer.HeatingArea, consumer.NumberOfPersons,
                consumer.ConsumerOwner, serviseInfo);
            var receipt = new Receipt
            {
                PersonalAccount = personalAccount,
                Date = DateTime.UtcNow.Date,
                PDF = pdf
            };
            await _invoiceRepository.Add(receipt);
            _logger.LogInformation("Added a new consumer to the database with PersonalAccount: {PersonalAccount}", receipt.PersonalAccount);
            return receipt;

        }
        public byte[] GeneratePdf(string personalAccount, string HouseAddress, int Flat, int HeatingArea, int NumberOfPersons, string ConsumerOwner, List<(string, int, int)> serviseInfo)
        {
            int x = 30;
            int y = 30;
            GlobalFontSettings.FontResolver = new CustomFontResolver();
            PdfDocument pdf = new PdfDocument();

            PdfPage page = pdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 14);

            gfx.DrawString($"Отримувач: {personalAccount}", font, XBrushes.Black, new XRect(x, y, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Вулиця: {HouseAddress}, Квартира: {Flat}", font, XBrushes.Black, new XRect(x, y, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
            y += 22;
            XRect rect = new XRect(x, y, page.Width - x*2, 50);
            XRect cellRect; 

           
            double cellWidth = rect.Width / 3;
            double cellHeight = rect.Height / 2;

            
            cellRect = new XRect(rect.X, rect.Y, cellWidth, cellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("Опалювальна площа", font, XBrushes.Black, cellRect, XStringFormats.Center);
            cellRect = new XRect(rect.X + cellWidth, rect.Y, cellWidth, cellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("Проживає осіб", font, XBrushes.Black, cellRect, XStringFormats.Center);
            cellRect = new XRect(rect.X + cellWidth * 2, rect.Y, cellWidth, cellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("ПIБ", font, XBrushes.Black, cellRect, XStringFormats.Center);

            
            cellRect = new XRect(rect.X, rect.Y + cellHeight, cellWidth, cellHeight);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString($"{HeatingArea}", font, XBrushes.Black, cellRect, XStringFormats.Center);
            cellRect = new XRect(rect.X + cellWidth, rect.Y + cellHeight, cellWidth, cellHeight);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString($"{NumberOfPersons}", font, XBrushes.Black, cellRect, XStringFormats.Center);
            cellRect = new XRect(rect.X + cellWidth * 2, rect.Y + cellHeight, cellWidth, cellHeight);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString($"{ConsumerOwner}", font, XBrushes.Black, cellRect, XStringFormats.Center);

            double secondTableY = rect.Y + rect.Height + 20; 
            XRect secondTableRect = new XRect(x, secondTableY, page.Width - x * 2, 50); 

            
            double secondTableCellWidth = secondTableRect.Width / 3;
            double secondTableCellHeight = secondTableRect.Height / serviseInfo.Count+1;

            
            cellRect = new XRect(secondTableRect.X, secondTableRect.Y, secondTableCellWidth, secondTableCellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("Назва послуги", font, XBrushes.Black, cellRect, XStringFormats.Center);

            cellRect = new XRect(secondTableRect.X + secondTableCellWidth, secondTableRect.Y, secondTableCellWidth, secondTableCellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("Тариф", font, XBrushes.Black, cellRect, XStringFormats.Center);

            cellRect = new XRect(secondTableRect.X + secondTableCellWidth * 2, secondTableRect.Y, secondTableCellWidth, secondTableCellHeight);
            gfx.DrawRectangle(XBrushes.LightGray, cellRect);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("До cплати", font, XBrushes.Black, cellRect, XStringFormats.Center);

            var sum = 0;
            
            for (int i = 0; i < serviseInfo.Count; i++)
            {
                cellRect = new XRect(secondTableRect.X, secondTableRect.Y + (i+1) * secondTableCellHeight, secondTableCellWidth, secondTableCellHeight);
                gfx.DrawRectangle(XPens.Black, cellRect);
                gfx.DrawString($"{serviseInfo[i].Item1}", font, XBrushes.Black, cellRect, XStringFormats.Center);

                cellRect = new XRect(secondTableRect.X + secondTableCellWidth, secondTableRect.Y + (i+1) * secondTableCellHeight, secondTableCellWidth, secondTableCellHeight);
                gfx.DrawRectangle(XPens.Black, cellRect);
                gfx.DrawString($"{serviseInfo[i].Item2}", font, XBrushes.Black, cellRect, XStringFormats.Center);

                cellRect = new XRect(secondTableRect.X + secondTableCellWidth * 2, secondTableRect.Y + (i+1) * secondTableCellHeight, secondTableCellWidth, secondTableCellHeight);
                gfx.DrawRectangle(XPens.Black, cellRect);
                gfx.DrawString($"{serviseInfo[i].Item3}", font, XBrushes.Black, cellRect, XStringFormats.Center);
                sum += serviseInfo[i].Item3;
            }

            cellRect = new XRect(secondTableRect.X + secondTableCellWidth, secondTableRect.Y + (serviseInfo.Count + 2) * secondTableCellHeight, secondTableCellWidth, secondTableCellHeight);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString("До cплати", font, XBrushes.Black, cellRect, XStringFormats.Center);
            cellRect = new XRect(secondTableRect.X + secondTableCellWidth * 2, secondTableRect.Y + (serviseInfo.Count + 2) * secondTableCellHeight, secondTableCellWidth, secondTableCellHeight);
            gfx.DrawRectangle(XPens.Black, cellRect);
            gfx.DrawString($"{sum}", font, XBrushes.Black, cellRect, XStringFormats.Center);

            gfx.DrawString($"Прохання сплатити протягом 10 днів, але не пізніше 20 числа!", font, XBrushes.Black, new XRect(x, (secondTableRect.Y + (serviseInfo.Count + 2) * secondTableCellHeight)+40, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

            //string filePath = "C:\\Users\\user\\Downloads\\квитанція.pdf"; 
            //pdf.Save(filePath);

            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
