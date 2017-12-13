using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using BraintreeHttp;
using Xunit;
using PayPal.Test;

namespace PayPal.PaymentExperience.Test
{

    public class WebProfileUpdateTest : TestHarness
    {

        [Fact]
        public async void TestWebProfileUpdateRequest()
        {
            // Create
            HttpResponse createResponse = await WebProfileCreateTest.createWebProfile();
            var expected = createResponse.Result<WebProfile>();
            expected.FlowConfig.BankTxnPendingUrl = "https://updated.com";

            // Update
            WebProfileUpdateRequest request = new WebProfileUpdateRequest(expected.Id);
            request.RequestBody(expected);

            HttpResponse response = await client().Execute(request);
            Assert.Equal((int) response.StatusCode, 204);
            
            // Get
            HttpResponse getResponse = await WebProfileGetTest.getWebProfile(expected.Id);
            Assert.Equal((int) getResponse.StatusCode, 200);
            var updated = getResponse.Result<WebProfile>();
            Assert.NotNull(updated);
            Assert.Equal(updated.FlowConfig.BankTxnPendingUrl, expected.FlowConfig.BankTxnPendingUrl);
        }
    }
}