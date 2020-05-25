
var system = { }

system.importing_enabled = __environment__has_importing;
system.import = __environment__import;

system.lifetime = { }
system.lifetime.enabled  = __lifetime__enabled;
system.lifetime.shutdown = __lifetime__shutdown;

system.log = { }
system.log.enabled     = __log__enabled;
system.log.information = __log__information;
system.log.warning     = __log__warning;
system.log.error       = __log__error;
system.log.critical    = __log__critical;
system.log.debug       = __log__debug;
system.log.trace       = __log__trace;
