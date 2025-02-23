﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.Security
{
    public static class ImageValidator
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                var Image = System.Drawing.Image.FromStream(file.OpenReadStream());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
