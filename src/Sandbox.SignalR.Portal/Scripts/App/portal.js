$(function() {
    var chat = $.connection.chatHub;

    chat.client.broadcastMessage = function(name, message) {
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();

        $('#discussion').append('<li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    };

    $('#displayname').val(prompt('Enter your name:', ''));
    $('#message').focus();

    $.connection.hub.stateChanged(function(change) {

        switch (change.newState) {
        case $.signalR.connectionState.reconnecting:
            $('#sendmessage')
                .attr('disabled', true)
                .off('.chat');

            break;
        case $.signalR.connectionState.connected:
            setTimeout(function() {
                chat.server.setUser({
                    name: $('#displayname').val()
                });

                $('#sendmessage')
                    .attr('disabled', false)
                    .on('click.chat', function() {

                        chat.server.send($('#message').val());
                        $('#message').val('').focus();

                        return false;
                    });
            }, 1000);

            break;
        case $.signalR.connectionState.disconnected:
            setTimeout(function() {
                $.connection.hub.start();

            }, 5000);

            break;
        }
    });

    $.connection.hub.start();
});