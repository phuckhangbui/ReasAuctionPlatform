using API.DTOs;
using API.Errors;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("/admin/dashboard/real/type")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<IEnumerable<RealEstateEachTypeDto>>> GetAmountRealEstateEachType()
        {
            var reals = await _dashboardService.GetAmountRealEstateEachType();
            return Ok(reals);
        }

        [HttpGet("/admin/dashboard/real/month")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<IEnumerable<RealEstateMonthDto>>> GetAmountRealEstateEachMonth()
        {
            var reals = await _dashboardService.GetAmountRealEstateEachMonth();
            return Ok(reals);
        }

        [HttpGet("/admin/dashboard/total")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<TotalUserAuctionReasDto>> GetTotalOfUserAndAuctionAndReas()
        {
            var totals = await _dashboardService.GetTotalOfUserAndAuctionAndReas();
            return Ok(totals);
        }

        [HttpGet("/admin/dashboard/news")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<IEnumerable<RealEstateMonthDto>>> GetNewsInDashboard()
        {
            var news = await _dashboardService.Get3NewNewsInDashboard();
            return Ok(news);
        }

        [HttpGet("/admin/dashboard/totalrevenue")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<double>> GetTotalRevenue()
        {
            var totalrevenue = await _dashboardService.GetTotalRevenue();
            return Ok(totalrevenue);
        }

        [HttpGet("/admin/dashboard/listusers")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<IEnumerable<UserJoinAuctionsDto>>> GetListUserJoinAuctions()
        {
            var users = await _dashboardService.GetListUserJoinAuctions();
            return Ok(users);
        }

        [HttpGet("/admin/dashboard/staffs")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<int>> GetStaffActive()
        {
            var users = await _dashboardService.GetStaffActive();
            return Ok(users);
        }
    }
}
