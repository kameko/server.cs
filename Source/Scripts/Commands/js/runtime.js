
var system = { }

if (typeof __environment__has_importing !== "undefined" && __environment__has_importing === true)
{
    system.import = __environment__import;
}

if (typeof __lifetime__enabled !== "undefined" && __lifetime__enabled === true)
{
    system.lifetime = { }
    system.lifetime.shutdown = __lifetime__shutdown;
}

if (typeof __log__enabled !== "undefined" && __log__enabled === true)
{
    system.log = { }
    system.log.enabled     = __log__enabled;
    system.log.information = __log__information;
    system.log.warning     = __log__warning;
    system.log.error       = __log__error;
    system.log.critical    = __log__critical;
    system.log.debug       = __log__debug;
    system.log.trace       = __log__trace;
}
