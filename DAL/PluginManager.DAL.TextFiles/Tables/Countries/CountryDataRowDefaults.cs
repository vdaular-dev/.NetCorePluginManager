﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: CountryDataRowDefaults.cs
 *
 *  Purpose:  Default initialisation values for country table
 *
 *  Date        Name                Reason
 *  18/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class CountryDataRowDefaults : ITableDefaults<CountryDataRow>
    {
        public long PrimarySequence => 0;

        public long SecondarySequence => 0;

        public ushort Version => 1;

        List<CountryDataRow> ITableDefaults<CountryDataRow>.InitialData(ushort version)
        {
            if (version == 1)
                return GetVersion1Data();

            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is initial load data and does not need to be retianed in memory as only used at startup")]
        private List<CountryDataRow> GetVersion1Data()
        {
            List<CountryDataRow> initialData = new()
			{
                new CountryDataRow() { Code = "ZZ", Name = "Unknown Country", Visible = false },
                new CountryDataRow() { Code = "GB", Name = "United Kingdom" },
                new CountryDataRow() { Code = "US", Name = "United States of America" },
                new CountryDataRow() { Code = "AF", Name = "Afghanistan" },
                new CountryDataRow() { Code = "AL", Name = "Albania" },
                new CountryDataRow() { Code = "DZ", Name = "Algeria" },
                new CountryDataRow() { Code = "AS", Name = "American Samoa" },
                new CountryDataRow() { Code = "AD", Name = "Andorra" },
                new CountryDataRow() { Code = "AO", Name = "Angola" },
                new CountryDataRow() { Code = "AI", Name = "Anguilla" },
                new CountryDataRow() { Code = "AQ", Name = "Antarctica" },
                new CountryDataRow() { Code = "AG", Name = "Antigua and Barbuda" },
                new CountryDataRow() { Code = "AR", Name = "Argentina" },
                new CountryDataRow() { Code = "AM", Name = "Armenia" },
                new CountryDataRow() { Code = "AW", Name = "Aruba" },
                new CountryDataRow() { Code = "AU", Name = "Australia" },
                new CountryDataRow() { Code = "AT", Name = "Austria" },
                new CountryDataRow() { Code = "AZ", Name = "Azerbaijan" },
                new CountryDataRow() { Code = "BS", Name = "Bahamas" },
                new CountryDataRow() { Code = "BH", Name = "Bahrain" },
                new CountryDataRow() { Code = "BD", Name = "Bangladesh" },
                new CountryDataRow() { Code = "BB", Name = "Barbados" },
                new CountryDataRow() { Code = "BY", Name = "Belarus" },
                new CountryDataRow() { Code = "BE", Name = "Belgium" },
                new CountryDataRow() { Code = "BZ", Name = "Belize" },
                new CountryDataRow() { Code = "BJ", Name = "Benin" },
                new CountryDataRow() { Code = "BM", Name = "Bermuda" },
                new CountryDataRow() { Code = "BT", Name = "Bhutan" },
                new CountryDataRow() { Code = "BO", Name = "Bolivia" },
                new CountryDataRow() { Code = "BA", Name = "Bosnia and Herzegovina" },
                new CountryDataRow() { Code = "BW", Name = "Botswana" },
                new CountryDataRow() { Code = "BV", Name = "Bouvet Island" },
                new CountryDataRow() { Code = "BR", Name = "Brazil" },
                new CountryDataRow() { Code = "IO", Name = "British Indian Ocean Territory" },
                new CountryDataRow() { Code = "BN", Name = "Brunei Darussalam" },
                new CountryDataRow() { Code = "BG", Name = "Bulgaria" },
                new CountryDataRow() { Code = "BF", Name = "Burkina Faso" },
                new CountryDataRow() { Code = "BI", Name = "Burundi" },
                new CountryDataRow() { Code = "KH", Name = "Cambodia" },
                new CountryDataRow() { Code = "CM", Name = "Cameroon" },
                new CountryDataRow() { Code = "CA", Name = "Canada" },
                new CountryDataRow() { Code = "CV", Name = "Cape Verde" },
                new CountryDataRow() { Code = "KY", Name = "Cayman Islands" },
                new CountryDataRow() { Code = "CF", Name = "Central African Republic" },
                new CountryDataRow() { Code = "TD", Name = "Chad" },
                new CountryDataRow() { Code = "CL", Name = "Chile" },
                new CountryDataRow() { Code = "CN", Name = "China" },
                new CountryDataRow() { Code = "CX", Name = "Christmas Island" },
                new CountryDataRow() { Code = "CC", Name = "Cocos (Keeling) Islands" },
                new CountryDataRow() { Code = "CO", Name = "Colombia" },
                new CountryDataRow() { Code = "KM", Name = "Comoros" },
                new CountryDataRow() { Code = "CG", Name = "Congo" },
                new CountryDataRow() { Code = "CK", Name = "Cook Islands" },
                new CountryDataRow() { Code = "CR", Name = "Costa Rica" },
                new CountryDataRow() { Code = "CI", Name = "Côte d'Ivoire" },
                new CountryDataRow() { Code = "HR", Name = "Croatia" },
                new CountryDataRow() { Code = "CU", Name = "Cuba" },
                new CountryDataRow() { Code = "CY", Name = "Cyprus" },
                new CountryDataRow() { Code = "CZ", Name = "Czech Republic" },
                new CountryDataRow() { Code = "DK", Name = "Denmark" },
                new CountryDataRow() { Code = "DJ", Name = "Djibouti" },
                new CountryDataRow() { Code = "DM", Name = "Dominica" },
                new CountryDataRow() { Code = "DO", Name = "Dominican Republic" },
                new CountryDataRow() { Code = "TP", Name = "East Timor" },
                new CountryDataRow() { Code = "EC", Name = "Ecuador" },
                new CountryDataRow() { Code = "EG", Name = "Egypt" },
                new CountryDataRow() { Code = "SV", Name = "El salvador" },
                new CountryDataRow() { Code = "GQ", Name = "Equatorial Guinea" },
                new CountryDataRow() { Code = "ER", Name = "Eritrea" },
                new CountryDataRow() { Code = "EE", Name = "Estonia" },
                new CountryDataRow() { Code = "ET", Name = "Ethiopia" },
                new CountryDataRow() { Code = "FK", Name = "Falkland Islands" },
                new CountryDataRow() { Code = "FO", Name = "Faroe Islands" },
                new CountryDataRow() { Code = "FJ", Name = "Fiji" },
                new CountryDataRow() { Code = "FI", Name = "Finland" },
                new CountryDataRow() { Code = "FR", Name = "France" },
                new CountryDataRow() { Code = "GF", Name = "French Guiana" },
                new CountryDataRow() { Code = "PF", Name = "French Polynesia" },
                new CountryDataRow() { Code = "TF", Name = "French Southern Territories" },
                new CountryDataRow() { Code = "GA", Name = "Gabon" },
                new CountryDataRow() { Code = "GM", Name = "Gambia" },
                new CountryDataRow() { Code = "GE", Name = "Georgia" },
                new CountryDataRow() { Code = "DE", Name = "Germany" },
                new CountryDataRow() { Code = "GH", Name = "Ghana" },
                new CountryDataRow() { Code = "GI", Name = "Gibraltar" },
                new CountryDataRow() { Code = "GR", Name = "Greece" },
                new CountryDataRow() { Code = "GL", Name = "Greenland" },
                new CountryDataRow() { Code = "GD", Name = "Grenada" },
                new CountryDataRow() { Code = "GP", Name = "Guadeloupe" },
                new CountryDataRow() { Code = "GU", Name = "Guam" },
                new CountryDataRow() { Code = "GT", Name = "Guatemala" },
                new CountryDataRow() { Code = "GN", Name = "Guinea" },
                new CountryDataRow() { Code = "GW", Name = "Guinea-Bissau" },
                new CountryDataRow() { Code = "GY", Name = "Guyana" },
                new CountryDataRow() { Code = "HT", Name = "Haiti" },
                new CountryDataRow() { Code = "HM", Name = "Heard Island and McDonald Islands" },
                new CountryDataRow() { Code = "VA", Name = "Holy See (Vatican City State)" },
                new CountryDataRow() { Code = "HN", Name = "Honduras" },
                new CountryDataRow() { Code = "HK", Name = "Hong Kong" },
                new CountryDataRow() { Code = "HU", Name = "Hungary" },
                new CountryDataRow() { Code = "IS", Name = "Iceland" },
                new CountryDataRow() { Code = "IN", Name = "India" },
                new CountryDataRow() { Code = "ID", Name = "Indonesia" },
                new CountryDataRow() { Code = "IR", Name = "Iran" },
                new CountryDataRow() { Code = "IQ", Name = "Iraq" },
                new CountryDataRow() { Code = "IE", Name = "Ireland" },
                new CountryDataRow() { Code = "IL", Name = "Israel" },
                new CountryDataRow() { Code = "IT", Name = "Italy" },
                new CountryDataRow() { Code = "JM", Name = "Jamaica" },
                new CountryDataRow() { Code = "JP", Name = "Japan" },
                new CountryDataRow() { Code = "JO", Name = "Jordan" },
                new CountryDataRow() { Code = "KZ", Name = "Kazakstan" },
                new CountryDataRow() { Code = "KE", Name = "Kenya" },
                new CountryDataRow() { Code = "KI", Name = "Kiribati" },
                new CountryDataRow() { Code = "KW", Name = "Kuwait" },
                new CountryDataRow() { Code = "KG", Name = "Kyrgystan" },
                new CountryDataRow() { Code = "LA", Name = "Lao" },
                new CountryDataRow() { Code = "LV", Name = "Latvia" },
                new CountryDataRow() { Code = "LB", Name = "Lebanon" },
                new CountryDataRow() { Code = "LS", Name = "Lesotho" },
                new CountryDataRow() { Code = "LR", Name = "Liberia" },
                new CountryDataRow() { Code = "LY", Name = "Libyan Arab Jamahiriya" },
                new CountryDataRow() { Code = "LI", Name = "Liechtenstein" },
                new CountryDataRow() { Code = "LT", Name = "Lithuania" },
                new CountryDataRow() { Code = "LU", Name = "Luxembourg" },
                new CountryDataRow() { Code = "MO", Name = "Macau" },
                new CountryDataRow() { Code = "MK", Name = "Macedonia (FYR)" },
                new CountryDataRow() { Code = "MG", Name = "Madagascar" },
                new CountryDataRow() { Code = "MW", Name = "Malawi" },
                new CountryDataRow() { Code = "MY", Name = "Malaysia" },
                new CountryDataRow() { Code = "MV", Name = "Maldives" },
                new CountryDataRow() { Code = "ML", Name = "Mali" },
                new CountryDataRow() { Code = "MT", Name = "Malta" },
                new CountryDataRow() { Code = "MH", Name = "Marshall Islands" },
                new CountryDataRow() { Code = "MQ", Name = "Martinique" },
                new CountryDataRow() { Code = "MR", Name = "Mauritania" },
                new CountryDataRow() { Code = "MU", Name = "Mauritius" },
                new CountryDataRow() { Code = "YT", Name = "Mayotte" },
                new CountryDataRow() { Code = "MX", Name = "Mexico" },
                new CountryDataRow() { Code = "FM", Name = "Micronesia" },
                new CountryDataRow() { Code = "MD", Name = "Moldova" },
                new CountryDataRow() { Code = "MC", Name = "Monaco" },
                new CountryDataRow() { Code = "MN", Name = "Mongolia" },
                new CountryDataRow() { Code = "MS", Name = "Montserrat" },
                new CountryDataRow() { Code = "MA", Name = "Morocco" },
                new CountryDataRow() { Code = "MZ", Name = "Mozambique" },
                new CountryDataRow() { Code = "MM", Name = "Myanmar" },
                new CountryDataRow() { Code = "NA", Name = "Namibia" },
                new CountryDataRow() { Code = "NR", Name = "Nauru" },
                new CountryDataRow() { Code = "NP", Name = "Nepal" },
                new CountryDataRow() { Code = "NL", Name = "Netherlands" },
                new CountryDataRow() { Code = "AN", Name = "Netherlands Antilles" },
                new CountryDataRow() { Code = "NT", Name = "Neutral Zone" },
                new CountryDataRow() { Code = "NC", Name = "New Caledonia" },
                new CountryDataRow() { Code = "NZ", Name = "New Zealand" },
                new CountryDataRow() { Code = "NI", Name = "Nicaragua" },
                new CountryDataRow() { Code = "NE", Name = "Niger" },
                new CountryDataRow() { Code = "NG", Name = "Nigeria" },
                new CountryDataRow() { Code = "NU", Name = "Niue" },
                new CountryDataRow() { Code = "NF", Name = "Norfolk Island" },
                new CountryDataRow() { Code = "KP", Name = "North Korea" },
                new CountryDataRow() { Code = "MP", Name = "Northern Mariana Islands" },
                new CountryDataRow() { Code = "NO", Name = "Norway" },
                new CountryDataRow() { Code = "OM", Name = "Oman" },
                new CountryDataRow() { Code = "PK", Name = "Pakistan" },
                new CountryDataRow() { Code = "PW", Name = "Palau" },
                new CountryDataRow() { Code = "PA", Name = "Panama" },
                new CountryDataRow() { Code = "PG", Name = "Papua New Guinea" },
                new CountryDataRow() { Code = "PY", Name = "Paraguay" },
                new CountryDataRow() { Code = "PE", Name = "Peru" },
                new CountryDataRow() { Code = "PH", Name = "Philippines" },
                new CountryDataRow() { Code = "PN", Name = "Pitcairn" },
                new CountryDataRow() { Code = "PL", Name = "Poland" },
                new CountryDataRow() { Code = "PT", Name = "Portugal" },
                new CountryDataRow() { Code = "PR", Name = "Puerto Rico" },
                new CountryDataRow() { Code = "QA", Name = "Qatar" },
                new CountryDataRow() { Code = "RE", Name = "Reunion" },
                new CountryDataRow() { Code = "RO", Name = "Romania" },
                new CountryDataRow() { Code = "RU", Name = "Russian Federation" },
                new CountryDataRow() { Code = "RW", Name = "Rwanda" },
                new CountryDataRow() { Code = "SH", Name = "Saint Helena" },
                new CountryDataRow() { Code = "KN", Name = "Saint Kitts and Nevis" },
                new CountryDataRow() { Code = "LC", Name = "Saint Lucia" },
                new CountryDataRow() { Code = "PM", Name = "Saint Pierre and Miquelon" },
                new CountryDataRow() { Code = "VC", Name = "Saint Vincent and the Grenadines" },
                new CountryDataRow() { Code = "WS", Name = "Samoa" },
                new CountryDataRow() { Code = "SM", Name = "San Marino" },
                new CountryDataRow() { Code = "ST", Name = "Sao Tome and Principe" },
                new CountryDataRow() { Code = "SA", Name = "Saudi Arabia" },
                new CountryDataRow() { Code = "SN", Name = "Senegal" },
                new CountryDataRow() { Code = "SC", Name = "Seychelles" },
                new CountryDataRow() { Code = "SL", Name = "Sierra Leone" },
                new CountryDataRow() { Code = "SG", Name = "Singapore" },
                new CountryDataRow() { Code = "SK", Name = "Slovakia" },
                new CountryDataRow() { Code = "SI", Name = "Slovenia" },
                new CountryDataRow() { Code = "SB", Name = "Solomon Islands" },
                new CountryDataRow() { Code = "SO", Name = "Somalia" },
                new CountryDataRow() { Code = "ZA", Name = "South Africa" },
                new CountryDataRow() { Code = "GS", Name = "South Georgia" },
                new CountryDataRow() { Code = "KR", Name = "South Korea" },
                new CountryDataRow() { Code = "ES", Name = "Spain" },
                new CountryDataRow() { Code = "LK", Name = "Sri Lanka" },
                new CountryDataRow() { Code = "SD", Name = "Sudan" },
                new CountryDataRow() { Code = "SR", Name = "Suriname" },
                new CountryDataRow() { Code = "SJ", Name = "Svalbard and Jan Mayen Islands" },
                new CountryDataRow() { Code = "SZ", Name = "Swaziland" },
                new CountryDataRow() { Code = "SE", Name = "Sweden" },
                new CountryDataRow() { Code = "CH", Name = "Switzerland" },
                new CountryDataRow() { Code = "SY", Name = "Syria" },
                new CountryDataRow() { Code = "TW", Name = "Taiwan" },
                new CountryDataRow() { Code = "TJ", Name = "Tajikistan" },
                new CountryDataRow() { Code = "TZ", Name = "Tanzania" },
                new CountryDataRow() { Code = "TH", Name = "Thailand" },
                new CountryDataRow() { Code = "TG", Name = "Togo" },
                new CountryDataRow() { Code = "TK", Name = "Tokelau" },
                new CountryDataRow() { Code = "TO", Name = "Tonga" },
                new CountryDataRow() { Code = "TT", Name = "Trinidad and Tobago" },
                new CountryDataRow() { Code = "TN", Name = "Tunisia" },
                new CountryDataRow() { Code = "TR", Name = "Turkey" },
                new CountryDataRow() { Code = "TM", Name = "Turkmenistan" },
                new CountryDataRow() { Code = "TC", Name = "Turks and Caicos Islands" },
                new CountryDataRow() { Code = "TV", Name = "Tuvalu" },
                new CountryDataRow() { Code = "UG", Name = "Uganda" },
                new CountryDataRow() { Code = "UA", Name = "Ukraine" },
                new CountryDataRow() { Code = "AE", Name = "United Arab Emirates" },
                new CountryDataRow() { Code = "UM", Name = "United States Minor Outlying Islands" },
                new CountryDataRow() { Code = "UY", Name = "Uruguay" },
                new CountryDataRow() { Code = "UZ", Name = "Uzbekistan" },
                new CountryDataRow() { Code = "VU", Name = "Vanuatu" },
                new CountryDataRow() { Code = "VE", Name = "Venezuela" },
                new CountryDataRow() { Code = "VN", Name = "Viet Nam" },
                new CountryDataRow() { Code = "VG", Name = "Virgin Islands (British)" },
                new CountryDataRow() { Code = "VI", Name = "Virgin Islands (U.S.)" },
                new CountryDataRow() { Code = "WF", Name = "Wallis and Futuna Islands" },
                new CountryDataRow() { Code = "EH", Name = "Western Sahara" },
                new CountryDataRow() { Code = "YE", Name = "Yemen" },
                new CountryDataRow() { Code = "YU", Name = "Yugoslavia" },
                new CountryDataRow() { Code = "ZR", Name = "Zaire" },
                new CountryDataRow() { Code = "ZM", Name = "Zambia" },
                new CountryDataRow() { Code = "ZW", Name = "Zimbabwe" },
                new CountryDataRow() { Code = "AX", Name = "Aland Islands" },
                new CountryDataRow() { Code = "EU", Name = "European Union" },
                new CountryDataRow() { Code = "GG", Name = "Guernsey" },
                new CountryDataRow() { Code = "IM", Name = "Isle of Man" },
                new CountryDataRow() { Code = "JE", Name = "Jersey" },
                new CountryDataRow() { Code = "ME", Name = "Montenegro" },
                new CountryDataRow() { Code = "MF", Name = "Saint Martin" },
                new CountryDataRow() { Code = "PS", Name = "Palestinian Territory Occupied" },
                new CountryDataRow() { Code = "RS", Name = "Serbia" },
                new CountryDataRow() { Code = "TL", Name = "Timor-leste" },
                new CountryDataRow() { Code = "CW", Name = "Curaçao" }
            };


            return initialData;
        }
    }
}
