
function on_message_received(message)
{
    system.log.information("From JS: " + message.content);
    return true;
}
