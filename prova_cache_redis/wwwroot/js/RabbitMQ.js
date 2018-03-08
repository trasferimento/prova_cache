
$(document).ready(function () {

    $("#risposta_LanciaProducer").text("");
    $("#risposta_LanciaConsumer").text("");

    $("#btnLanciaProducer").button();
    $("#btnLanciaConsumer").button();
    
    //pressione bottone LanciaPRoducer
    $("#btnLanciaProducer").click(function (e) {

        
        // faccio DELETE
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


    //submit del form inserimento nuovo tema
    $("#nuovo_tema").submit(function (event) {

        event.preventDefault(); //blocco funzionamento usuale del form

        var miaurl = "/StocksTemi/PostNuovoTema";

        // serializzo tutti i campi di input in JSON
        // il nome dei vari parametri verra' preso dal campo name=x del <input name=x>
        //var datijson = $(this).serializeArray(); 
        //serializzo campi +  saltare i campi vuoti di testo ( rompono alla validazione del modello MVC, in quanto invia par1= e lascia bianco)
        var datijson = $("#nuovo_tema :input").filter(function (index, element) {
            return $(element).val() != "";
        }).serializeArray();
        //console.log(datijson);

        //faccio POST  dei dati del form
        $.post(miaurl, datijson, function (risposta) {
            //se ho degli errori 
            if (risposta.errore !== undefined) {
                $("#risposta_nuovo_tema").text(risposta.errore);
                console.log(risposta.errore);
                $("#risposta_nuovo_tema").addClass("ui-state-error");
                $("#risposta_nuovo_tema_icona").addClass("ui-icon ui-icon-alert");
            }
            //se tutto ok, ricarico la pagina 
            if (risposta.successo !== undefined) {
                $("#risposta_nuovo_tema").text(risposta.successo);
                console.log(risposta.successo);
                location.reload();
            }
        }, 'json') //risposta in json

    })


});