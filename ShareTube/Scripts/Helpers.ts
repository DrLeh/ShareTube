function stringFormat(format : string, ...args : string[]) {
    return format.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match
        ;
    });
};
function getParameterByName(src, name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(src);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}