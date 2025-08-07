using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IMapper _mapper;
        public PurchaseController(IRepository<Purchase> purchaseRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PurchaseDto>> GetAll()
        {
            var purchases = _purchaseRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<PurchaseDto>>(purchases);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<PurchaseDto> Create(CreatePurchaseDto dto)
        {
            var purchase = _mapper.Map<Purchase>(dto);
            purchase.Id = Guid.NewGuid().ToString();
            _purchaseRepository.Add(purchase);
            var result = _mapper.Map<PurchaseDto>(purchase);
            return CreatedAtAction(nameof(GetAll), new { id = purchase.Id }, result);
        }
    }
}
