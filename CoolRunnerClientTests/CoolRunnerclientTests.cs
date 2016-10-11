using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CRClient;
using CRClient.Enums;
using CRClient.Exceptions;
using CRClient.HttpClient;
using CRClient.Models;
using CRClient.Responses;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CoolRunnerClientTests
{
    public class CoolRunnerClientTests
    {
        private Mock<IHttpCoolRunnerClient> _httpMock;
        private CoolRunnerClientMock _sut;
        private readonly string _baseUrl = "https://api.coolrunner.dk/v1";

        #region Models

        protected ShipmentModel ValidModel => new ShipmentModel
        {
            ReceiverName = "John Doe",
            ReceiverAttention = "",
            ReceiverStreet1 = "Hidden street 7",
            ReceiverStreet2 = "",
            ReceiverZipcode = "1234",
            ReceiverCity = "Secret City",
            ReceiverCountry = "SC",
            ReceiverNotifySms = "",
            ReceiverNotifyEmail = "JohnDoe@Secret.com",
            SenderName = "Jane Doe",
            SenderAttention = "",
            SenderStreet1 = "Hidden street 5",
            SenderStreet2 = "",
            SenderZipcode = "1234",
            SenderCity = "Secret City",
            SenderCountry = "SC",
            SenderPhone = "",
            SenderEmail = "JaneDoe@Secret.com",
            Droppoint = false,
            DroppointId = "",
            DroppointName = "",
            DroppointStreet1 = "",
            DroppointZipcode = "",
            DroppointCity = "",
            DroppointCountry = "",
            Length = 300,
            Width = 200,
            Height = 400,
            Weight = 3000,
            Carrier = Carrier.Pdk,
            CarrierProduct = CarrierProduct.Private,
            CarrierService = CarrierService.DeliveryPackage,
            Insurance = false,
            InsuranceValue = 0,
            InsuranceCurrency = "",
            CustomsValue = 0,
            CustomsCurrency = "",
            Reference = "",
            Description = "",
            Comment = "",
            LabelFormat = LabelFormat.LabelPrint
        };

        protected ShipmentModel InvalidModel => new ShipmentModel
        {
            ReceiverName = "",
            ReceiverAttention = "",
            ReceiverStreet1 = "Hidden street 7",
            ReceiverStreet2 = "",
            ReceiverZipcode = "1234",
            ReceiverCity = "Secret City",
            ReceiverCountry = "SC",
            ReceiverNotifySms = "",
            ReceiverNotifyEmail = "JohnDoe@Secret.com",
            SenderName = "Jane Doe",
            SenderAttention = "",
            SenderStreet1 = "Hidden street 5",
            SenderStreet2 = "",
            SenderZipcode = "1234",
            SenderCity = "Secret City",
            SenderCountry = "SC",
            SenderPhone = "",
            SenderEmail = "JaneDoe@Secret.com",
            Droppoint = false,
            DroppointId = "",
            DroppointName = "",
            DroppointStreet1 = "",
            DroppointZipcode = "",
            DroppointCity = "",
            DroppointCountry = "",
            Length = 300,
            Width = 200,
            Height = 400,
            Weight = 3000,
            Carrier = Carrier.Pdk,
            CarrierProduct = CarrierProduct.Private,
            CarrierService = CarrierService.DeliveryPackage,
            Insurance = false,
            InsuranceValue = 0,
            InsuranceCurrency = "",
            CustomsValue = 0,
            CustomsCurrency = "",
            Reference = "",
            Description = "",
            Comment = "",
            LabelFormat = LabelFormat.LabelPrint
        };

        protected DroppointModel ValidDroppointModel = new DroppointModel
        {
            CountryCode = "DK",
            Street = "Test street",
            NumberOfDroppoints = 10,
            PostCode = "1234"
        };

        #endregion

        [SetUp]
        public void BeforeEach()
        {
            _httpMock = new Mock<IHttpCoolRunnerClient>();
            _sut = new CoolRunnerClientMock(_httpMock.Object);
            _sut.SetCredentials("test", "test");
            _sut.SetCallerIdentifier("ClientTests");
        }

        [Test]
        public async Task CreateShipmentAsync_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { order_id = 1, grand_total_excl_tax = 40, grand_total_incl_tax = 50, shipment_id = 10000, price_incl_tax = 50, price_excl_tax = 40, reference = "132546", package_number = 45621346587952, labelless_code = "", pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA...", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });
            
            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = await _sut.CreateShipmentAsync(ValidModel);

            // Assert
            Assert.IsTrue(ValidModel.Validate().IsValid);
            Assert.IsTrue(ValidModel.ReceiverNotify);

            Assert.AreEqual(1, response.OrderId);
            Assert.AreEqual(40, response.GrandTotalExclTax);
            Assert.AreEqual(50, response.GrandTotalInclTax);
            Assert.AreEqual(10000, response.ShipmentId);
            Assert.AreEqual(40, response.PriceExclTax);
            Assert.AreEqual(50, response.PriceInclTax);
            Assert.AreEqual("132546", response.Reference);
            Assert.AreEqual("", response.LabellessCode);
            Assert.AreEqual(45621346587952, response.PackageNumber);
            Assert.AreEqual("JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA...", response.PdfBase64);
            Assert.AreEqual("https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB", response.PdfLink);
        }

        [Test]
        public void CreateShipmentAsync_InvalidModel_ErrorResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "No valid shipment to be processed", result = new { status = "error", message = "Shipment could not be created: Direction Import not currently supported, Home country: DK, Sender country: FI, ReceiverCountry: DK", reference = "test_pdk_business" }, unique_id = "Ny0xNDMwNzMzNDEyLTgwODE", error_link = "https://api.coolrunner.dk/v1/error/Ny0xNDMwNzMzNDEyLTgwODE" });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(async () => await _sut.CreateShipmentAsync(InvalidModel), Throws.Exception.TypeOf<CoolRunnerException>());

            Assert.IsFalse(InvalidModel.Validate().IsValid);
            Assert.IsTrue(InvalidModel.ReceiverNotify);
        }

        [Test]
        public void CreateShipment_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { order_id = 1, grand_total_excl_tax = 40, grand_total_incl_tax = 50, shipment_id = 10000, price_incl_tax = 50, price_excl_tax = 40, reference = "132546", package_number = 45621346587952, labelless_code = "", pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA...", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });


            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = _sut.CreateShipment(ValidModel);

            // Assert
            Assert.IsTrue(ValidModel.Validate().IsValid);
            Assert.IsTrue(ValidModel.ReceiverNotify);

            Assert.AreEqual(1, response.OrderId);
            Assert.AreEqual(40, response.GrandTotalExclTax);
            Assert.AreEqual(50, response.GrandTotalInclTax);
            Assert.AreEqual(10000, response.ShipmentId);
            Assert.AreEqual(40, response.PriceExclTax);
            Assert.AreEqual(50, response.PriceInclTax);
            Assert.AreEqual("132546", response.Reference);
            Assert.AreEqual("", response.LabellessCode);
            Assert.AreEqual(45621346587952, response.PackageNumber);
            Assert.AreEqual("JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA...", response.PdfBase64);
            Assert.AreEqual("https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB", response.PdfLink);
        }

        [Test]
        public void CreateShipment_InvalidModel_ErrorResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "No valid shipment to be processed", result = new { status = "error", message = "Shipment could not be created: Direction Import not currently supported, Home country: DK, Sender country: FI, ReceiverCountry: DK", reference = "test_pdk_business" }, unique_id = "Ny0xNDMwNzMzNDEyLTgwODE", error_link = "https://api.coolrunner.dk/v1/error/Ny0xNDMwNzMzNDEyLTgwODE" });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(() => _sut.CreateShipment(InvalidModel), Throws.Exception.TypeOf<CoolRunnerException>());

            Assert.IsFalse(InvalidModel.Validate().IsValid);
            Assert.IsTrue(InvalidModel.ReceiverNotify);
        }

        [Test]
        public async Task GetPriceAsync_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { zone_from = "DK", zone_to = "DK", weight_from = 0, weight_to = 10000, title = "Post Danmark", price_incl_tax = 60, price_excl_tax = 48, reference = "test_pdk" } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = await _sut.GetPriceAsync(ValidModel);

            // Assert
            Assert.IsTrue(ValidModel.Validate().IsValid);
            Assert.IsTrue(ValidModel.ReceiverNotify);

            Assert.AreEqual("DK", response.ZoneFrom);
            Assert.AreEqual("DK", response.ZoneTo);
            Assert.AreEqual(0, response.WeightFrom);
            Assert.AreEqual(10000, response.WeightTo);
            Assert.AreEqual("Post Danmark", response.Title);
            Assert.AreEqual(48, response.PriceExclTax);
            Assert.AreEqual(60, response.PriceInclTax);
            Assert.AreEqual("test_pdk", response.Reference);

        }

        [Test]
        public void GetPriceAsync_InvalidModel_ErrorResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "Could not calculate price for shipment", result = new { error = "Could not calculate price for shipment", reference = "test_pdk_business" }, unique_id = "Ny0xNDMwNzMzNDEyLTgwODE", error_link = "https://api.coolrunner.dk/v1/error/Ny0xNDMwNzMzNDEyLTgwODE" });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };


            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(async () => await _sut.GetPriceAsync(InvalidModel), Throws.Exception.TypeOf<CoolRunnerException>());

            Assert.IsFalse(InvalidModel.Validate().IsValid);
            Assert.IsTrue(InvalidModel.ReceiverNotify);
        }

        [Test]
        public void GetPrice_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { zone_from = "DK", zone_to = "DK", weight_from = 0, weight_to = 10000, title = "Post Danmark", price_incl_tax = 60, price_excl_tax = 48, reference = "test_pdk" } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = _sut.GetPrice(ValidModel);

            // Assert
            Assert.IsTrue(ValidModel.Validate().IsValid);
            Assert.IsTrue(ValidModel.ReceiverNotify);

            Assert.AreEqual("DK", response.ZoneFrom);
            Assert.AreEqual("DK", response.ZoneTo);
            Assert.AreEqual(0, response.WeightFrom);
            Assert.AreEqual(10000, response.WeightTo);
            Assert.AreEqual("Post Danmark", response.Title);
            Assert.AreEqual(48, response.PriceExclTax);
            Assert.AreEqual(60, response.PriceInclTax);
            Assert.AreEqual("test_pdk", response.Reference);
        }

        [Test]
        public void GetPrice_InvalidModel_ErrorResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "Could not calculate price for shipment", result = new { error = "Could not calculate price for shipment", reference = "test_pdk_business" }, unique_id = "Ny0xNDMwNzMzNDEyLTgwODE", error_link = "https://api.coolrunner.dk/v1/error/Ny0xNDMwNzMzNDEyLTgwODE" });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(() => _sut.GetPrice(InvalidModel), Throws.Exception.TypeOf<CoolRunnerException>());

            Assert.IsFalse(InvalidModel.Validate().IsValid);
            Assert.IsTrue(InvalidModel.ReceiverNotify);
        }

        [Test]
        public async Task GetShipmentInfoAsync_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { shipment_id = 1, order_id = 1, package_number = 1234567898, pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = await _sut.GetShipmentInfoAsync(1);

            // Assert
            Assert.AreEqual(1, response.ShipmentId);
            Assert.AreEqual(1, response.OrderId);
            Assert.AreEqual(1234567898, response.PackageNumber);
            Assert.AreEqual("JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", response.PdfBase64);
            Assert.AreEqual("https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB", response.PdfLink);
        }

        [Test]
        public void GetShipmentInfoAsync_InvalidModel_Exception()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "", result = new { shipment_id = 1, order_id = 1, package_number = 1234567898, pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(async () => await _sut.GetShipmentInfoAsync(1), Throws.Exception.TypeOf<CoolRunnerException>());
        }

        [Test]
        public void GetShipmentInfo_ValidModel_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { shipment_id = 1, order_id = 1, package_number = 1234567898, pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = _sut.GetShipmentInfo(1);

            // Assert
            Assert.AreEqual(1, response.ShipmentId);
            Assert.AreEqual(1, response.OrderId);
            Assert.AreEqual(1234567898, response.PackageNumber);
            Assert.AreEqual("JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", response.PdfBase64);
            Assert.AreEqual("https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB", response.PdfLink);
        }

        [Test]
        public void GetShipmentInfo_InvalidModel_Exception()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "", result = new { shipment_id = 1, order_id = 1, package_number = 1234567898, pdf_base64 = "JVBERi0xLjMNCiXi48/TDQoxIDAgb2JqDQo8PA", pdf_link = "https://api.coolrunner.dk/v1/pdf/Ny0xNjQtMTc5LTA1NzAwMDAyMDEzMTAwMDEyNB" } });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(() => _sut.GetShipmentInfo(1), Throws.Exception.TypeOf<CoolRunnerException>());
        }

        [TestCase(Carrier.Dao)]
        [TestCase(Carrier.Pdk)]
        [TestCase(Carrier.Gls)]
        public async Task GetDroppointsAsync_ValidModel_SuccessResponse(Carrier carrier)
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { droppoint_id = "1", carrier = carrier.ToString(), name = "Super spar", distance = 1223, address = new { street = "Test street 1", postal_code = "1234", city = "Test City", country_code = "DK" }, coordinate = new { latitude = "57.0433", longitude = "9.96646" }, opening_hours = new { mo = new { from = "10.00", to = "20.00" } } } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync($"{_baseUrl}/droppoints/{carrier}", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = await _sut.GetDroppointsAsync(carrier, ValidDroppointModel);

            // Assert
            Assert.AreEqual(1, response.DroppointId);
            Assert.AreEqual(carrier, response.Carrier);
            Assert.AreEqual("Super spar", response.Name);
            Assert.AreEqual(1223, response.Distance);
            Assert.AreEqual("DK", response.Address.CountryCode);
            Assert.AreEqual("Test City", response.Address.City);
            Assert.AreEqual("Test street 1", response.Address.Street);
            Assert.AreEqual("1234", response.Address.ZipCode);
            Assert.AreEqual("57.0433", response.Coordinate.Latitude);
            Assert.AreEqual("9.96646", response.Coordinate.Longitude);
            Assert.AreEqual(0, response.OpeningHours.Days.Count);
        }

        [TestCase(Carrier.Dao)]
        [TestCase(Carrier.Pdk)]
        [TestCase(Carrier.Gls)]
        public void GetDroppointsAsync_InvalidModel_Exception(Carrier carrier)
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "", result = new { droppoint_id = "1", carrier = carrier.ToString(), name = "Super spar", distance = 1223, address = new { street = "Test street 1", postal_code = "1234", city = "Test City", country_code = "DK" }, coordinate = new { latitude = "57.0433", longitude = "9.96646" }, opening_hours = new { mo = new { from = "10.00", to = "20.00" } } } });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync($"{_baseUrl}/droppoints/{carrier}", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(async () => await _sut.GetDroppointsAsync(carrier, ValidDroppointModel), Throws.Exception.TypeOf<CoolRunnerException>());
        }

        [TestCase(Carrier.Dao)]
        [TestCase(Carrier.Pdk)]
        [TestCase(Carrier.Gls)]
        public void GetDroppoints_ValidModel_SuccessResponse(Carrier carrier)
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "ok", message = "", result = new { droppoint_id = "1", carrier = carrier.ToString(), name = "Super spar", distance = 1223, address = new { street = "Test street 1", postal_code = "1234", city = "Test City", country_code = "DK" }, coordinate = new { latitude = "57.0433", longitude = "9.96646" }, opening_hours = new { mo = new { from = "10.00", to = "20.00" } } } });

            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync($"{_baseUrl}/droppoints/{carrier}", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(successResponseMessage);

            // Act
            var response = _sut.GetDroppoints(carrier, ValidDroppointModel);

            // Assert
            Assert.AreEqual(1, response.DroppointId);
            Assert.AreEqual(carrier, response.Carrier);
            Assert.AreEqual("Super spar", response.Name);
            Assert.AreEqual(1223, response.Distance);
            Assert.AreEqual("DK", response.Address.CountryCode);
            Assert.AreEqual("Test City", response.Address.City);
            Assert.AreEqual("Test street 1", response.Address.Street);
            Assert.AreEqual("1234", response.Address.ZipCode);
            Assert.AreEqual("57.0433", response.Coordinate.Latitude);
            Assert.AreEqual("9.96646", response.Coordinate.Longitude);
            Assert.AreEqual(0, response.OpeningHours.Days.Count);
        }

        [TestCase(Carrier.Dao)]
        [TestCase(Carrier.Pdk)]
        [TestCase(Carrier.Gls)]
        public void GetDroppoints_InvalidModel_Exception(Carrier carrier)
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { status = "error", message = "", result = new { droppoint_id = "1", carrier = carrier.ToString(), name = "Super spar", distance = 1223, address = new { street = "Test street 1", postal_code = "1234", city = "Test City", country_code = "DK" }, coordinate = new { latitude = "57.0433", longitude = "9.96646" }, opening_hours = new { mo = new { from = "10.00", to = "20.00" } } } });

            var errorResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.PostAsync($"{_baseUrl}/droppoints/{carrier}", It.IsAny<FormUrlEncodedContent>()))
                .ReturnsAsync(errorResponseMessage);

            // Act & Assert
            Assert.That(() => _sut.GetDroppoints(carrier, ValidDroppointModel), Throws.Exception.TypeOf<CoolRunnerException>());
        }

        [Test]
        public async Task DeletePackageLabelAsync_InvoiseHasNotBeenSent_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new {});

            var successHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync($"{_baseUrl}/shipment/delete/1")).ReturnsAsync(successHttpResponseMessage);

            // Act
            var response = await _sut.DeletePackageLabelAsync(1);

            // Assert
            Assert.IsTrue(response);
        }

        [Test]
        public void DeletePackageLabelAsync_LabelCanNotBeDeleted_Exception()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { });

            var errorHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync($"{_baseUrl}/shipment/delete/1")).ReturnsAsync(errorHttpResponseMessage);

            // Act & Assert
            Assert.That(async () => await _sut.DeletePackageLabelAsync(1), Throws.Exception.TypeOf<CoolRunnerException>());
        }

        [Test]
        public void DeletePackageLabel_InvoiseHasNotBeenSent_SuccessResponse()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { });

            var successHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync($"{_baseUrl}/shipment/delete/1")).ReturnsAsync(successHttpResponseMessage);

            // Act
            var response = _sut.DeletePackageLabel(1);

            // Assert
            Assert.IsTrue(response);
        }

        [Test]
        public void DeletePackageLabel_LabelCanNotBeDeleted_Exception()
        {
            // Arrange
            var jsonResponse = JObject.FromObject(new { });

            var errorHttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(jsonResponse.ToString())
            };

            _httpMock.Setup(x => x.GetAsync($"{_baseUrl}/shipment/delete/1")).ReturnsAsync(errorHttpResponseMessage);

            // Act & Assert
            Assert.That(() => _sut.DeletePackageLabel(1), Throws.Exception.TypeOf<CoolRunnerException>());
        }
    }
}