﻿function recognize() {


    var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
    var value = "listening now  ....";
    if (isChrome) {
        const texts = document.querySelector(".texts");

        window.SpeechRecognition = window.speechRecognition || window.webkitSpeechRecognition;

        const recognition = new SpeechRecognition();
        recognition.interimResults = true;
        $("#speech_to_text").val(value);

        recognition.addEventListener("result", (e) => {

            const text = Array.from(e.results)
                .map((result) => result[0])
                .map((result) => result.transcript)
                .join("");


            if (e.results[0].isFinal) {
                value = $("#speech_to_text").val();
                value = text;
                $("#speech_to_text").val(value);

            }
        });



        recognition.start();

    } else {
        value = "use chrome if you want to utilize the voice search";
        $("#speech_to_text").val(value);
    }





}