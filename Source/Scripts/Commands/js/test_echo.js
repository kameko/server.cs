
function on_message_received(message)
{
    system.log.information("From JS: " + message.content);
    if (message.content === "shutdown")
    {
        system.lifetime.shutdown();
    }
    return true;
}
