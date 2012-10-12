

(function( window, undefined ) {

    var Globalize;

    if ( typeof require !== "undefined"
	    && typeof exports !== "undefined"
	    && typeof module !== "undefined" ) {
	    // Assume CommonJS
	    Globalize = require( "globalize" );
    } else {
	    // Global variable
	    Globalize = window.Globalize;
    }

    Globalize.addCultureInfo("vi-VI", "default", {
        // A unique name for the culture in the form <language code>-<country/region code>
        name: "vi-VI",
        // the name of the culture in the english language
        englishName: "VietNam",
        // the name of the culture in its own language
        nativeName: "Việt Nam",
        // whether the culture uses right-to-left text
        isRTL: false,
        // "language" is used for so-called "specific" cultures.
        // For example, the culture "es-CL" means "Spanish, in Chili".
        // It represents the Spanish-speaking culture as it is in Chili,
        // which might have different formatting rules or even translations
        // than Spanish in Spain. A "neutral" culture is one that is not
        // specific to a region. For example, the culture "es" is the generic
        // Spanish culture, which may be a more generalized version of the language
        // that may or may not be what a specific culture expects.
        // For a specific culture like "es-CL", the "language" field refers to the
        // neutral, generic culture information for the language it is using.
        // This is not always a simple matter of the string before the dash.
        // For example, the "zh-Hans" culture is netural (Simplified Chinese).
        // And the "zh-SG" culture is Simplified Chinese in Singapore, whose lanugage
        // field is "zh-CHS", not "zh".
        // This field should be used to navigate from a specific culture to it's
        // more general, neutral culture. If a culture is already as general as it
        // can get, the language may refer to itself.
        language: "vi-VI",
        // numberFormat defines general number formatting rules, like the digits in
        // each grouping, the group separator, and how negative numbers are displayed.
        numberFormat: {
            // [negativePattern]
            // Note, numberFormat.pattern has no "positivePattern" unlike percent and currency,
            // but is still defined as an array for consistency with them.
            //   negativePattern: one of "(n)|-n|- n|n-|n -"
            pattern: [ "-n" ],
            // number of decimal places normally shown
            decimals: 2,
            // string that separates number groups, as in 1,000,000
            ",": ".",
            // string that separates a number from the fractional portion, as in 1.99
            ".": ",",
            // array of numbers indicating the size of each number group.
            // TODO: more detailed description and example
            groupSizes: [ 3 ],
            // symbol used for positive numbers
            "+": "+",
            // symbol used for negative numbers
            "-": "-",
            // symbol used for NaN (Not-A-Number)
            NaN: "NaN",
            // symbol used for Negative Infinity
            negativeInfinity: "Âm vô cùng",
            // symbol used for Positive Infinity
            positiveInfinity: "Dương vô cùng",
            percent: {
                // [negativePattern, positivePattern]
                //   negativePattern: one of "-n %|-n%|-%n|%-n|%n-|n-%|n%-|-% n|n %-|% n-|% -n|n- %"
                //   positivePattern: one of "n %|n%|%n|% n"
                pattern: [ "-n %", "n %" ],
                // number of decimal places normally shown
                decimals: 2,
                // array of numbers indicating the size of each number group.
                // TODO: more detailed description and example
                groupSizes: [ 3 ],
                // string that separates number groups, as in 1,000,000
                ",": ".",
                // string that separates a number from the fractional portion, as in 1.99
                ".": ",",
                // symbol used to represent a percentage
                symbol: "%"
            },
            currency: {
                // [negativePattern, positivePattern]
                //   negativePattern: one of "($n)|-$n|$-n|$n-|(n$)|-n$|n-$|n$-|-n $|-$ n|n $-|$ n-|$ -n|n- $|($ n)|(n $)"
                //   positivePattern: one of "$n|n$|$ n|n $"
                pattern: [ "($n)", "$n" ],
                // number of decimal places normally shown
                decimals: 2,
                // array of numbers indicating the size of each number group.
                // TODO: more detailed description and example
                groupSizes: [ 3 ],
                // string that separates number groups, as in 1,000,000
                ",": ".",
                // string that separates a number from the fractional portion, as in 1.99
                ".": ",",
                // symbol used to represent currency
                symbol: "đ"
            }
        },
        // calendars defines all the possible calendars used by this culture.
        // There should be at least one defined with name "standard", and is the default
        // calendar used by the culture.
        // A calendar contains information about how dates are formatted, information about
        // the calendar's eras, a standard set of the date formats,
        // translations for day and month names, and if the calendar is not based on the Gregorian
        // calendar, conversion functions to and from the Gregorian calendar.
        calendars: {
            standard: {
                // name that identifies the type of calendar this is
                name: "Lịch dương",
                // separator of parts of a date (e.g. "/" in 11/05/1955)
                "/": "/",
                // separator of parts of a time (e.g. ":" in 05:44 PM)
                ":": ":",
                // the first day of the week (0 = Sunday, 1 = Monday, etc)
                firstDay: 1,
                days: {
                    // full day names
                    names: [ 'Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
                    // abbreviated day names
                    namesAbbr: [ 'CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7' ],
                    // shortest day names
                    namesShort: [ 'CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7' ]
                },
                months: {
                    // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                    names: [ 'Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu', 'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai', "" ],
                    // abbreviated month names
                    namesAbbr: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12', ""]
                },
                // AM and PM designators in one of these forms:
                // The usual view, and the upper and lower case versions
                //   [ standard, lowercase, uppercase ]
                // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
                //   null
                AM: [ "Sáng", "sáng", "SÁNG" ],
                PM: [ "Chiều", "chiều", "CHIỀU" ],
                eras: [
                    // eras in reverse chronological order.
                    // name: the name of the era in this culture (e.g. A.D., C.E.)
                    // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
                    // offset: offset in years from gregorian calendar
                    {
                        "name": "S.A.",
                        "start": null,
                        "offset": 0
                    }
                ],
                // when a two digit year is given, it will never be parsed as a four digit
                // year greater than this year (in the appropriate era for the culture)
                // Set it as a full year (e.g. 2029) or use an offset format starting from
                // the current year: "+19" would correspond to 2029 if the current year 2010.
                twoDigitYearMax: 2029,
                // set of predefined date and time patterns used by the culture
                // these represent the format someone in this culture would expect
                // to see given the portions of the date that are shown.
                patterns: {
                    // short date pattern
                    d: "d/M/yyyy",
                    // long date pattern
                    D: "dddd, dd MMMM, yyyy",
                    // short time pattern
                    t: "h:mm tt",
                    // long time pattern
                    T: "h:mm:ss tt",
                    // long date, short time pattern
                    f: "dddd, dd MMMM, yyyy h:mm tt",
                    // long date, long time pattern
                    F: "dddd, dd MMMM, yyyy h:mm:ss tt",
                    // month/day pattern
                    M: "dd MMMM",
                    // month/year pattern
                    Y: "MMMM yyyy",
                    // S is a sortable format that does not vary by culture
                    S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss"
                }
                // optional fields for each calendar:
                /*
                monthsGenitive:
                    Same as months but used when the day preceeds the month.
                    Omit if the culture has no genitive distinction in month names.
                    For an explaination of genitive months, see http://blogs.msdn.com/michkap/archive/2004/12/25/332259.aspx
                convert:
                    Allows for the support of non-gregorian based calendars. This convert object is used to
                    to convert a date to and from a gregorian calendar date to handle parsing and formatting.
                    The two functions:
                        fromGregorian( date )
                            Given the date as a parameter, return an array with parts [ year, month, day ]
                            corresponding to the non-gregorian based year, month, and day for the calendar.
                        toGregorian( year, month, day )
                            Given the non-gregorian year, month, and day, return a new Date() object
                            set to the corresponding date in the gregorian calendar.
                */
            }
        },
        // For localized strings
        messages: {}
    });

}( this ));
