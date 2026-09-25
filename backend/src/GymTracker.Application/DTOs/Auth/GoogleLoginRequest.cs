using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.DTOs.Auth
{
    public record GoogleLoginRequest(string IdToken);
}
