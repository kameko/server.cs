
var system = { }

if (typeof __environment__has_importing !== "undefined" && __environment__has_importing === true)
{
    system.import = __environment__import;
}

system.lifetime = { }
if (typeof __lifetime__enabled !== "undefined" && __lifetime__enabled === true)
{
    system.lifetime.shutdown = __lifetime__shutdown;
}

system.log = { }
system.log.enabled = false;
if (typeof __log__enabled !== "undefined" && __log__enabled === true)
{
    system.log.enabled     = true;
    system.log.information = __log__information;
    system.log.warning     = __log__warning;
    system.log.error       = __log__error;
    system.log.critical    = __log__critical;
    system.log.debug       = __log__debug;
    system.log.trace       = __log__trace;
}

system.storage = { }
system.storage.enabled = false;
if (typeof __storage__enabled !== "undefined" && __storage__enabled === true)
{
    system.storage.get = __storage__get;
    system.storage.set = __storage__set;
}

system.storage.global = { }
system.storage.global.enabled = false;
if (typeof __storage__global__enabled !== "undefined" && __storage__global__enabled === true)
{
    system.storage.global.get = __storage__global__get;
    system.storage.global.set = __storage__global__set;
}
