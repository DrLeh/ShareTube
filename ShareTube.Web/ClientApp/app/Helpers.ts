﻿function getParameterByName(src: string, name:string) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(src);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}