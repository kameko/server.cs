
var system = { }

system.import = __environment__import;

system.lifetime = { }
system.lifetime.shutdown = __lifetime__shutdown;

system.log = { }
system.log.information = __log__information;
system.log.warning     = __log__warning;
system.log.error       = __log__error;
system.log.critical    = __log__critical;
system.log.debug       = __log__debug;
system.log.trace       = __log__trace;
