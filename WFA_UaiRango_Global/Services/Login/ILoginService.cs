﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Uairango.Dto;

namespace WFA_UaiRango_Global.Services.Login
{
    public interface ILoginService
    {
        Task<LoginUaiRangoResponseDto> LoginAsync();
    }
}
