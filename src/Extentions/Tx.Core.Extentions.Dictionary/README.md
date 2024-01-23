Usage:


ccType = new Dictionary<string, char?>()
{
    {"visa", 'V'},
    {"mastercard", 'M'},
    {"discover", 'D'},
    {"american express", 'AMEX'}
}.WithDefaultValue("None");

var result = ccType[<some strinvg var>];

