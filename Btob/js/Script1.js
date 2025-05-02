//Web service Business central
//<afficher commandes vente
$(document).on('click', '#commande', function (e) {

    affichercommande();

});

function affichercommande() {
    productlist = document.getElementById('productList');
    productlist.innerHTML = "";
    result = document.getElementById('result');

    $.ajax({
        type: 'POST',
        url: '../Login/affichercommande',
        dataType: 'json',
        async: false,
        data: {},
        success: function (data) {
            var totalTTC = 0;
            var totalTVA = 0;
            var html = "";

            html += "<div class='container-fluid'>"; 
            html += "<h2 class='text-center mt-3'>Liste des Commandes</h2>";
            html += "<p class='text-center text-muted'>Voici un aperçu de toutes les commandes enregistrées.</p><hr>";

            if (data && data.length > 0) {
                $.each(data, function (index, commande) {
                    var montantTVA = commande.MontantTTC - commande.Montant;
                    totalTTC += commande.MontantTTC;
                    totalTVA += montantTVA;
                });

                html += "<div class='text-end me-3'>"; 
                html += "<h4><strong>Total TVA :</strong> " + totalTVA.toFixed(3) + " </h4>";
                html += "<h4><strong>Total TTC :</strong> " + totalTTC.toFixed(3) + " </h4>";
                html += "</div>";

                html += "<div class='table-responsive'>"; 
                html += "<table id='mytable' class='table table-bordered table-striped w-100'>"; 
                html += "<thead class='table-dark'><tr>";
                html += "<th>N°</th>";
                html += "<th>Code Client</th>";
                html += "<th class='text-end'>Montant</th>";
                html += "<th class='text-end'>Montant TVA</th>";
                html += "<th class='text-end'>Montant TTC</th>";
                html += "<th>Date</th>";
                html += "<th>Détail</th>";
                html += "<th>Imprimer</th>";
                html += "</tr></thead><tbody>";

                $.each(data, function (index, commande) {
                    var montantTVA = commande.MontantTTC - commande.Montant;

                    html += "<tr>";
                    html += "<td>" + commande.Numero + "</td>";
                    html += "<td>" + commande.NumeroClient + "</td>";
                    html += "<td class='text-end'>" + commande.Montant.toFixed(3) + "</td>";
                    html += "<td class='text-end'>" + montantTVA.toFixed(3) + "</td>";
                    html += "<td class='text-end'>" + commande.MontantTTC.toFixed(3) + "</td>";
                    html += "<td>" + commande.Date + "</td>";
                    html += `<td><button onclick="DetailCdeVente('${commande.Numero}');" class='btn btn-primary btn-sm' data-toggle='modal' data-target='#edit1'><i class='glyphicon glyphicon-pencil'></i> Détail</button></td>`;
                    html += `<td><button onclick="genererCommandePDF('${commande.Numero}');" class='btn btn-danger btn-sm'  ><i class='glyphicon glyphicon-print'></i> Imprimer</button></td>`;
                    html += "</tr>";
                });

                html += "</tbody></table></div>";
                html += "</div>";
                result.innerHTML = html;
            } else {
                result.innerHTML = "<div class='text-center text-muted'>Aucune commande trouvée.</div>";
            }
        }
    });
}

function expeditionvente() {
    productlist = document.getElementById('productList');
    productlist.innerHTML = "";
    result = document.getElementById('result');

    $.ajax({
        type: 'POST',
        url: '../Login/expeditionvente',
        dataType: 'json',
        async: false,
        data: {},
        success: function (data) {

            if (!data || data.length === 0) {
                result.innerHTML = "<div class='text-center text-muted'>Aucune expédition trouvée.</div>";
                return;
            }

            let html = "<div class='container-fluid'>"; 
            html += "<h2 class='text-center mt-3'>Liste des Expéditions</h2>";
            html += "<p class='text-center text-muted'>Voici un aperçu de toutes les expéditions enregistrées.</p><hr>";

            html += "<div class='table-responsive'>";
            html += "<table class='table table-bordered table-striped w-100'>"; 
            html += "<thead><tr><th>N°</th><th>Code Client</th><th>Nom Client</th><th>Date</th><th>Détail</th><th>Imprimer</th></tr></thead><tbody>";

            $.each(data, function (index, commande) {
                html += `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.Nom}</td>
                    <td>${commande.NumeroClient}</td>
                    <td>${commande.Date}</td>
                    <td>
                        <button onclick="DetailExpVente('${commande.Numero}');" class="btn btn-primary btn-xs">
                            <span class="glyphicon glyphicon-pencil"></span> Détail
                        </button>
                    </td>
                    <td> 
                        <button onclick="genererExpeditionPDF('${commande.Numero}');" class="btn btn-danger btn-xs">
                            <span class="glyphicon glyphicon-print"></span> Imprimer
                        </button>
                    </td>
                </tr>`;
            });

            html += "</tbody></table>";
            html += "</div>";
            html += "</div>";

            result.innerHTML = html;
        }
    });
}



function facturevente(page = 1, perPage = 10) {
    productlist = document.getElementById('productList');
    productlist.innerHTML = "";
    result = document.getElementById('result');

    $.ajax({
        type: 'POST',
        url: '../Login/facturevente',
        dataType: 'json',
        async: false,
        data: {},
        success: function (data) {
            if (!data || data.length === 0) {
                result.innerHTML = "<div class='text-center text-muted'>Aucune facture trouvée.</div>";
                return;
            }

            let totalItems = data.length;
            let totalPages = Math.ceil(totalItems / perPage);
            let start = (page - 1) * perPage;
            let end = start + perPage;
            let paginatedData = data.slice(start, end);

            let html = "<div class='container-fluid'>";
            html += "<h2 class='text-center mt-3'>Liste des Factures </h2>";
            html += "<p class='text-center text-muted'>Voici un aperçu de toutes les factures enregistrées</p><hr>";

            html += "<div class='table-responsive'>";
            html += "<table class='table table-bordered table-striped w-100'>";
            html += "<thead><tr><th>N°</th><th>Numéro Client</th><th>Nom Client</th><th>Montant</th><th>Montant TVA</th><th>Date</th><th>Détail</th><th>Imprimer</th></tr></thead><tbody>";

            $.each(paginatedData, function (index, commande) {
                html += `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.NumeroClient}</td>
                    <td>${commande.Nom}</td>
                    <td>${commande.Montant}</td>
                    <td>${commande.MontantTVA}</td>
                    <td>${commande.Date}</td>
                    <td>
                      <button onclick="DetailFactureVente('${commande.Numero}');" class="btn btn-primary btn-xs">
                        <span class="glyphicon glyphicon-pencil"></span> Détail
                      </button>
                    </td>
                    <td> 
                        <button onclick="genererFacturePDF('${commande.Numero}');" class="btn btn-danger btn-xs">
                            <span class="glyphicon glyphicon-print"></span> Imprimer
                        </button>
                    </td>
                </tr>`;
            });

            html += "</tbody></table></div>";

            html += "<nav aria-label='Page navigation'><ul class='pagination justify-content-center'>";
            for (let i = 1; i <= totalPages; i++) {
                html += `<li class='page-item ${i === page ? "active" : ""}'>
                            <a class='page-link' href='#' onclick='facturevente(${i}, ${perPage})'>${i}</a>
                         </li>`;
            }
            html += "</ul></nav></div>";

            result.innerHTML = html;
        }
    });
}

function avoirvente() {
    productlist = document.getElementById('productList');
    productlist.innerHTML = "";
    result = document.getElementById('result');

    $.ajax({
        type: 'POST',
        url: '../Login/avoirvente',
        dataType: 'json',
        async: false,
        data: {},
        success: function (data) {

            if (!data || data.length === 0) {
                result.innerHTML = "<div class='text-center text-muted'>Aucun avoir trouvé.</div>";
                return;
            }

            let html = "<div class='container-fluid'>"; 
            html += "<h2 class='text-center mt-3'>Liste des Avoirs de Vente</h2>";
            html += "<p class='text-center text-muted'>Voici un aperçu de tous les avoirs enregistrés.</p><hr>";

            html += "<div class='table-responsive'>";
            html += "<table class='table table-bordered table-striped w-100'>"; 
            html += "<thead><tr><th>N°</th><th>Numéro Client</th><th>Nom Client</th><th>Montant</th><th>Montant TVA</th><th>Date</th><th>Détail</th><th>Imprimer</th></tr></thead><tbody>";

            $.each(data, function (index, commande) {
                html += `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.NumeroClient}</td>
                    <td>${commande.Nom}</td>
                    <td>${commande.Montant}</td>
                    <td>${commande.MontantTVA}</td>
                    <td>${commande.Date}</td>
                    <td>
                      <button onclick="DetailAvoirVente('${commande.Numero}');" class="btn btn-primary btn-xs">
                        <span class="glyphicon glyphicon-pencil"></span> Détail
                      </button>

                    </td>
                    <td> 
                        <button onclick="genererAvoirPDF('${commande.Numero}');" class="btn btn-danger btn-xs">
                            <span class="glyphicon glyphicon-print"></span> Imprimer
                        </button>
                    </td>
                </tr>`;
            });

            html += "</tbody></table>";
            html += "</div>"; 
            html += "</div>"; 

            result.innerHTML = html;
        }
    });
}
function DetailCdeVente(ID) {
    $.ajax({
        type: 'POST',
        url: '../Login/DetailCdeVente',
        data: { ID: ID },
        dataType: 'json',
        success: function (data) {
            $('#commandeDetailsBody').empty();

            $.each(data, function (index, commande) {
                var row = `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.Description}</td>
                    <td>${commande.Quantite}</td>
                    <td>${commande.Prix.toFixed(2)}</td>
                    <td>${commande.MontantTVA.toFixed(2)}</td>
                    <td>${commande.Montant.toFixed(2)}</td>
                </tr>`;
                $('#commandeDetailsBody').append(row);
            });

            $('#edit1').modal('show');
        },
        error: function (xhr, status, error) {
            alert("Une erreur est survenue : " + error);
        }
    });
}
function DetailExpVente(ID) {
    $.ajax({
        type: 'POST',
        url: '../Login/DetailExpVente',
        data: { ID: ID },
        dataType: 'json',
        success: function (data) {
            $('#ExpCommande').empty();

            $.each(data, function (index, commande) {
                var row = `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.Description}</td>
                    <td>${commande.Quantite}</td>
                </tr>`;
                $('#ExpCommande').append(row);
            });

            $('#edit2').modal('show');
        },
        error: function (xhr, status, error) {
            alert("Une erreur est survenue : " + error);
        }
    });
}
function DetailFactureVente(ID) {
    $.ajax({
        type: 'POST',
        url: '../Login/DetailFactureVente',
        data: { ID: ID },
        dataType: 'json',
        success: function (data) {
            $('#commandeFacture').empty();

            $.each(data, function (index, commande) {
                var row = `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.Description}</td>
                    <td>${commande.Quantite}</td>
                    <td>${commande.MontantTVA.toFixed(2)}</td>
                    <td>${commande.Montant.toFixed(2)}</td>
                </tr>`;
                $('#commandeFacture').append(row);
            });

            $('#edit3').modal('show');
        },
        error: function (xhr, status, error) {
            alert("Une erreur est survenue : " + error);
        }
    });
}
function DetailAvoirVente(ID) {
    $.ajax({
        type: 'POST',
        url: '../Login/DetailAvoirVente',
        data: { ID: ID },
        dataType: 'json',
        success: function (data) {
            $('#avoir').empty();

            $.each(data, function (index, commande) {
                var row = `<tr>
                    <td>${commande.Numero}</td>
                    <td>${commande.Description}</td>
                    <td>${commande.Quantite}</td>
                    <td>${commande.MontantTVA.toFixed(2)}</td>
                    <td>${commande.Montant.toFixed(2)}</td>
                </tr>`;
                $('#avoir').append(row);
            });

            $('#edit4').modal('show');
        },
        error: function (xhr, status, error) {
            alert("Une erreur est survenue : " + error);
        }
    });
}
function genererCommandePDF(ID) {
    window.open('../Login/ImprimerCdeVente?ID=' + ID, '_blank');
}
function genererExpeditionPDF(ID) {
    window.open('../Login/ImprimerExpedition?ID=' + ID, '_blank');
}
function genererFacturePDF(ID) {
    window.open('../Login/ImprimerFacture?ID=' + ID, '_blank');
}
function genererAvoirPDF(ID) {
    window.open('../Login/ImprimerAvoir?ID=' + ID, '_blank');
}


//>Web service Business central

//a supprimer les fonctions web service .Net
////////Web service .NET
function GetPanier()
{
    result = document.getElementById('result');
    $.ajax({
        type: 'POST',
        url: '../Login/GetPanier',
        dataType: 'json',
        async: false,
        data: {
            

        },
        success: function (data) {
            var i = 0;
            var totalTTC = 0;
            if (data != '') {
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    html = "";
                }
                else {
               html = "<div class=\"container\">";
                    html += "<div class=\"row\">";
                    html += "<div class=\"col-md-12\">";
                    html += "<h4 style=\"color:black; \"></h4>";
                    html += "<div class=\"responsive-table-line\" style=\"margin:0px auto;\">";
                    html += " <table id=\"mytable\" class=\"table table-bordred table-striped\">";
                    html += " <thead><tr>";
                    html += "<th><input type=\"checkbox\" id=\"checkall\" hidden=\"hidden\" /></th>";
                    html += "<th style=\"width: 10%;display:none;\" align=\"left\"><h3> Ligne</h3></th>";
                    html += "<th style=\"width: 10%\" align=\"left\">Référence</th>";
                    html += "<th style=\"width: 60%\" align=\"left\">Désignation</th>";
                    html += "<th style=\"width: 10%\">Prix</th>";
                    html += "<th style=\"width: 10%\">Quantité</th>";
                    html += "<th>Modifier</th>";
                    html += "<th>Supprimer</th>";
                    html += "</tr></thead><tbody>";
                }

                $.each(data, function (index) {
                    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                        html += "  <ul class=\"list-group\">";

                        html += "<li class=\"list-group-item\" hidden=\"hidden\">" + data[index].NLigne + "</li>";

                        //  html += "<li class=\"list-group-item active\">" + data[index].Ref + "</li>";

                        html += "<li class=\"list-group-item\"><p>" + data[index].Designation + "</p><p> Prix : " + data[index].PUTTC.toFixed(3) + "</p><p> Quantité : " + data[index].quantite; + "  </p>";

                        // html += "<li class=\"list-group-item\">" + data[index].PUTTC.toFixed(3) + "</li>";

                        // html += "<li class=\"list-group-item\">" + data[index].quantite + "  ";
                        //   html += "<p data-placement=\"top\" data-toggle=\"tooltip\" title=\"Edit\"><button class=\"btn btn-primary btn-xs\" data-title=\"Edit\" data-toggle=\"modal\" data-target=\"#edit\"><span class=\"glyphicon glyphicon-pencil\"></span></button></p> ";
                        // html += "<p data-placement=\"top\" data-toggle=\"tooltip\" title=\"Delete\"><button class=\"btn btn-danger btn-xs\" data-title=\"Delete\" data-toggle=\"modal\" data-target=\"#delete\"><span class=\"glyphicon glyphicon-trash\"></span></button></p></li>";
                        html += '<p>   </p><button onclick="GetNL(\'' + data[index].NLigne + '\',\'' + data[index].Ref + '\',\'' + data[index].Designation + '\',\'' + data[index].PUTTC.toFixed(3)+ '\',\'' + data[index].quantite + '\');\" class=\"btn btn-primary btn-xs\" data-title=\"Edit\" data-toggle=\"modal\" data-target=\"#edit\"><span class=\"glyphicon glyphicon-pencil\"></span></button> ';
                        html += "<button  onclick=GetNLdelete(" + data[index].NLigne + "," + data[index].Ref + ") class=\"btn btn-danger btn-xs\" data-title=\"Delete\" data-toggle=\"modal\" data-target=\"#delete\"><span class=\"glyphicon glyphicon-trash\"></span></button></li>";
                        html += "</ul>";
                        totalTTC = totalTTC + (data[index].quantite * data[index].PUTTC);

                        html += "  <ul class=\"list-group\">";
                        html += "<li class=\"list-group-item\" hidden=\"hidden\">" + totalTTC + "</li>";

                        html += "</ul>";
                    }
                    else {

                        html += "<tr>";
                        html += "<td><input type=\"checkbox\" hidden=\"hidden\" class=\"checkthis\" /></td>";
                        html += "<td align=\"left\" style=\"display:none;\">" + data[index].NLigne + "</td>";
                        html += "<td align=\"left\" >" + data[index].Ref + "</td>";
                        html += "<td align=\"left\">" + data[index].Designation + " </td>";
                        html += "<td align=\"right\">" + data[index].PUTTC.toFixed(3) + "</td>";
                        html += "<td align=\"right\">" + data[index].quantite + "</td>";
                        html += '<td><p data-placement=\"top\" data-toggle=\"tooltip\" title=\"Edit\"><button onclick="GetNL(\'' + data[index].NLigne + '\',\'' + data[index].Ref + '\',\'' + data[index].Designation + '\',\'' + data[index].PUTTC.toFixed(3) + '\',\'' + data[index].quantite + '\');\"  class=\"btn btn-primary btn-xs\" data-title=\"Edit\" data-toggle=\"modal\" data-target=\"#edit\"><span class=\"glyphicon glyphicon-pencil\"></span></button></p></td>';

                        html += "<td><p data-placement=\"top\" data-toggle=\"tooltip\" title=\"Delete\"><button  onclick=GetNLdelete(" + data[index].NLigne + "," + data[index].Ref + ") class=\"btn btn-danger btn-xs\" data-title=\"Delete\" data-toggle=\"modal\" data-target=\"#delete\"><span class=\"glyphicon glyphicon-trash\"></span></button></p></td>";

                        html += "</tr>";

                        totalTTC = totalTTC + (data[index].quantite * data[index].PUTTC);
                     

                    }
                });
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                   }
                else {
                    html += "</tbody></table>";
                    html += "<div align=\"right\"><h4 align=\"Right\">Total HT : " + totalTTC.toFixed(3) + "     </h4>";
                    html += "<tr><td><h4 align=\"Right\">Total TVA :  0.000     </h4></td>";
                    html += "<td><h4 align=\"Right\">Total TTC : " + totalTTC.toFixed(3) + "     </h4></td>";
                    html += "<td><button onclick=validerCde() type=\"button\" class=\"btn btn-success\">Valider</button></td></div>";
                }

                result.innerHTML = html;
            } else {
                html = "<div></div>";
                result.innerHTML = html;

            }
        }

    });
}
function Addpanier(No, designation, montant,qte) {
   
  
    //  ,designation,montant
    $.ajax({
        type: 'POST',
        url: '../Login/Addpanier',
        dataType: 'json',
        async: false,
        data: {
            No: No,
            designation: designation,
            montant: montant,
            qte:qte
        },
        success: function (data) {
            var i = 0;
            if (data != '-1') {
                document.getElementById("nbreLignes").innerHTML = data;
               // alert(data);
                Response.Cookies["qtete"].Value = data;

            }
            else {
                alert('NON');
            }
        }

    });


}


function GetNLAdd() {
    const ref = $('#RefAdd').val();
    const designation = $('#desAdd').val();
    const prix = $('#PrixAdd').val().replace(',', '.');
    const quantite = $('#qteAdd').val();

    $.ajax({
        url: '/Login/Addpanier',
        type: 'POST',
        data: {
            No: ref,
            designation: designation,
            montant: prix,
            qte: quantite
        },
        success: function (response) {
            if (response.success) {
                $('#mytable').text(response.count);

                toastr.success(response.message);

                if (window.location.pathname.includes("Panier")) {
                    location.reload();
                }
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("Erreur lors de la communication avec le serveur");
        }
    });
}
function AddItem()
{
   
    No = document.getElementById('RefAdd').value;
    designation=  document.getElementById('desAdd').value;
    montant = document.getElementById('PrixAdd').value;
    qte = document.getElementById('qteAdd').value;
   
    Addpanier(No, designation, montant, qte)
   



}
function GetNLdelete(NL, ref) {
    document.getElementById('NLDEL').value = NL;
    document.getElementById('RefDEL').value = ref;
    //  alert(document.getElementById('RefDEL').value);
    //  alert(document.getElementById('NLDEL').value);

}
function DeleteItem() {
    var NL = document.getElementById('NLDEL').value;
    var Ref = document.getElementById('RefDEL').value;
    $.ajax({
        type: 'POST',
        url: '../Login/DeleteItem',
        dataType: 'json',
        async: false,
        data: {
            NL: NL,
            Ref: Ref
        },
        success: function (data) {
            if (data != '-1') {
               // Response.Cookies["qtete"].Value = data;
                document.getElementById("nbreLignes").innerHTML = data;
                GetPanier();
            } else { document.getElementById("nbreLignes").innerHTML = "0"; }
        }
    });
}
function UpdateItem(){

    var NL = document.getElementById('NL').value;
    var Ref = document.getElementById('Ref').value;
    var des=  document.getElementById('des').value;
    var prix = document.getElementById('prix').value;
    var qte = document.getElementById('qte').value;
    
    $.ajax({
        type: 'POST',
        url: '../Login/UpdateItem',
        dataType: 'json',
        async: false,
        data: {
            NL: NL,
            Ref: Ref,
            description: des,
            qte: qte,
            prix:prix 
        },
        success: function (data) {
            if (data == '1') {
               // alert('ok');
                GetPanier();
            }
            else
                alert(data);
        }
    });



}