using api.filters;
using api.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

public class PaymentInfoController: ControllerBase
{
    //todo create payment info (should have some enums that sets the payment type)
    //todo delete payment info
    //todo edit payment info 
    //todo get list of valid payment options user has created
    
    

    public PaymentInfoController()
    {
        
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/account/paymentoptions")]
    public ResponseDto GetPaymentOptions()
    {
        //todo should send a full list of all available payment options (show all enums for payment options)
        throw new NotImplementedException();
    }
    
    
    
}