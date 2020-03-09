// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var klient = document.getElementById("selected_item");
var rentBtn = document.querySelectorAll(".details_style");

/*rentBtn.forEach(x => {
    x.addEventListener("click", e => {
        e.preventDefault();
        var klientIndex = klient.options[klient.selectedIndex].value;
        var link = x.getAttribute("href");
        var myArr = link.split("");
        myArr.splice(-1, 1, klientIndex)
        link = myArr.join("");
        x.href = link;

        });
    });*/

function rent(id) {
    $.post("/RentedMovies/Rent", // this is url
        {
            UserId: document.getElementById("selected_item").value,
            TvShowId: id
        });
    var url = "";
    var rentId = "#rent_" + id;
    $(rentId).hide();
    var retId = "#ret_" + id;
    $(retId).show();
}

function ret(id) {
    $.post("/RentedMovies/Return",
        {
            UserId: document.getElementById("selected_item").value,
            TvShowId: id
        });
    var rentId = "#rent_" + id;
    $(rentId).show();
    var retId = "#ret_" + id;
    $(retId).hide();
}

function fnExcelReport() {

    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    tab = document.getElementById('headerTable'); // id of table

    var myRow = $('.table_styling')
    var last = myRow.find('th:last-child, td:last-child').remove()

    for (var j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        myRow[j].append(last[j])
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}

function myFunction() {
    var input = document.getElementById("myInput");
    var filter = input.value.toLowerCase();
    var row = document.querySelectorAll('.table_styling');
    var title = document.querySelectorAll('.title_style');

    for (i = 0; i < row.length; i++) {
        var textTitle = title[i].innerText;
        if (textTitle.toLowerCase().indexOf(filter) > -1) {
            row[i].style.display = "";
        } else {
            row[i].style.display = "none";
        }
    }
}

/*function fnExcelReport() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('headerTable'); // id of table

    for (var j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else                //other browser not tested on IE 11
    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

return (sa);
}*/