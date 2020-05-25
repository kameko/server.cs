
var itr  = 0;
var gitr = system.storage.get("gitr");

function on_message_received(message)
{
    if (gitr === null)
    {
        system.storage.set("gitr", "0");
    }
    gitr = system.storage.get("gitr");
    system.log.information("From JS(L" + itr + "/S" + gitr + "): " + message.content);
    itr++;
    system.storage.set("gitr", (Number(gitr) + 1).toString());
    if (message.content === "shutdown")
    {
        system.lifetime.shutdown();
    }
    return true;
}
