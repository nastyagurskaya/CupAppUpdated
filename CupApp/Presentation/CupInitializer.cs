using CupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Threading.Tasks;

namespace CupApp.Presentation
{
    public class CupInitializer
    {
        private CupContext _context;
        private List<string> countriesNames;

        public CupInitializer(CupContext context)
        {
            _context = context;
            RegionInfo countryReg = new RegionInfo(new CultureInfo("en-US", false).LCID);
            countriesNames = new List<string>();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                countryReg = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                string countryName = countryReg.DisplayName.ToString();
                if (!countriesNames.Contains(countryName))
                    countriesNames.Add(countryName);
            }
            countriesNames.Sort();

            countriesNames.ForEach(c => _countries.Add(new Country
            {
                CountryName = c
            }));
        }

        public async Task Seed()
        {
            if (!_context.Countries.Any())
            {
                _context.AddRange(_countries);
                await _context.SaveChangesAsync();
            }

        }

        List<Country> _countries = new List<Country>{};
    }
}