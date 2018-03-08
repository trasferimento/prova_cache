
$(document).ready(function () {

    $("#risposta_LanciaProducer").text("");
    $("#risposta_LanciaConsumer").text("");

    $("#btnLanciaProducer").button();
    $("#btnLanciaConsumer").button();
    
    //pressione bottone LanciaPRoducer
    $("#btnLanciaProducer").click(function (e) {
                
        // faccio GET
        var miaurl = "/RabbitMQ/LanciaProducer";
        var querystring = "?TemaInvestimentoId=" + 23;
        var miaurl_e_querystring = miaurl + querystring;
        $.ajax({
            url: miaurl_e_querystring,
            type: 'GET',
            dataType: "json", //mette un header nella richiesta x segnalare Accept dati in json
            success: function (risposta) {
                //$('#AjaxSpinner').hide();
                if (risposta.errore !== undefined) {
                    $("#risposta_LanciaProducer").text(risposta.errore);
                    console.log(risposta.errore);
                    $("#risposta_LanciaProducer").addClass("ui-state-error");
                    $("#risposta_LanciaProducer_icona").addClass("ui-icon ui-icon-alert");
                }
                //se tutto ok, ricarico la pagina 
                if (risposta.successo !== undefined) {
                    $("#risposta_LanciaProducer").text(risposta.successo);
                    console.log(risposta.successo);
                    //location.reload();
                }
            }, // fine successo
            error: function (xhr, ajaxOptions, thrownError) {
                //$('#AjaxSpinner').hide();
                console.log("Errore nella get " + xhr.status + " " + xhr.statusText);
                $("#risposta_LanciaProducer").text("Errore nella DELETE " + xhr.status + " " + xhr.statusText);
                $("#risposta_LanciaProducer").addClass("ui-state-error");
                $("#risposta_LanciaProducer_icona").addClass("ui-icon ui-icon-alert");
            } //error nella chiamata AJAX
        });//fine ajax

    }); //fine bottone cancella stock

    //pressione bottone LanciaConsumer
    $("#btnLanciaConsumer").click(function (e) {

        // faccio GET
        var miaurl = "/RabbitMQ/LanciaConsumer";
        var querystring = "?TemaInvestimentoId=" + 23;
        var miaurl_e_querystring = miaurl + querystring;
        $.ajax({
            url: miaurl_e_querystring,
            type: 'GET',
            dataType: "json", //mette un header nella richiesta x segnalare Accept dati in json
            success: function (risposta) {
                //$('#AjaxSpinner').hide();
                if (risposta.errore !== undefined) {
                    $("#risposta_LanciaConsumer").text(risposta.errore);
                    console.log(risposta.errore);
                    $("#risposta_LanciaConsumer").addClass("ui-state-error");
                    $("#risposta_LanciaConsumer_icona").addClass("ui-icon ui-icon-alert");
                }
                //se tutto ok, ricarico la pagina 
                if (risposta.successo !== undefined) {
                    $("#risposta_LanciaConsumer").text(risposta.successo);
                    console.log(risposta.successo);
                    //location.reload();
                }
            }, // fine successo
            error: function (xhr, ajaxOptions, thrownError) {
                //$('#AjaxSpinner').hide();
                console.log("Errore nella get " + xhr.status + " " + xhr.statusText);
                $("#risposta_LanciaConsumer").text("Errore nella DELETE " + xhr.status + " " + xhr.statusText);
                $("#risposta_LanciaConsumer").addClass("ui-state-error");
                $("#risposta_LanciaConsumer_icona").addClass("ui-icon ui-icon-alert");
            } //error nella chiamata AJAX
        });//fine ajax

    }); //fine bottone cancella stock

    

});