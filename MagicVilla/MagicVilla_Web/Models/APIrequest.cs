﻿using System.Security.AccessControl;
using static MagicVilla_Utility.SD;


namespace MagicVilla_Web.Models;

public class APIrequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string Token { get; set; }
}