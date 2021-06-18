const truckmanagerbaseurl = "https://localhost:44368/api/truck/";

$(document).ready(function () {
    truckmanager.load();

    $(".truck-btn-cancelar").click(function () {
        truckmanager.list();
    });
    $("#truck-list-btnnew").click(function () {
        truckmanager.add.open();
    });
    $("#truck-add-btnsave").click(function () {
        truckmanager.add.save();
    });
    $("#truck-edit-btnsave").click(function () {
        truckmanager.edit.save();
    });
});

var truckmanager = {
    ajax: function (type, url, data, fnsuccess, fnerror) {
        $.ajax({
            type: type,
            url: url,
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if ($.isFunction(fnsuccess)) {
                    fnsuccess(data);
                }
            },
            error: function (err) {
                if ($.isFunction(fnerror)) {
                    fnerror(err);
                }
            }
        });

    },
    load: function () {
        truckmanager.list();
    },
    showdiv: function (divid) {
        const divL = $("#truck-list");
        const divA = $("#truck-add");
        const divE = $("#truck-edit");
        divL.hide();
        divA.hide();
        divE.hide();
        $(`#${divid}`).show();
    },
    list: function () {
        const divid = "truck-list";

        truckmanager.ajax("GET", truckmanagerbaseurl, null, function (res) {
            if (!res.status) {
                console.error(res.errorMsg);
                return;
            }

            $(`#${divid}-table-body`).html("");
            $(`#${divid}-table-template`).tmpl(res.data).appendTo(`#${divid}-table-body`);

            $(`#${divid}-table`).DataTable();

            truckmanager.showdiv(divid);
        }, function (errr) {
            console.error("[get-edit]", err);
        });
    },
    add: {
        open: function () {
            const divid = "truck-add";
            const dateNow = new Date();
            console.log(dateNow.getFullYear());
            $(`#${divid}-name`).val("");
            $(`#${divid}-type`).val(0);
            $(`#${divid}-manufactureyear`).val(dateNow.getFullYear());
            $(`#${divid}-modelyear`).val(dateNow.getFullYear());

            truckmanager.showdiv(divid);
        },
        save: function () {
            const divid = "truck-add";

            var data = {
                id: 0,
                name: $(`#${divid}-name`).val(),
                modelType: Number($(`#${divid}-type`).val()),
                manufactureYear: Number($(`#${divid}-manufactureyear`).val()),
                modelYear: Number($(`#${divid}-modelyear`).val()),
            };
            console.log(data);

            truckmanager.ajax("POST", truckmanagerbaseurl, JSON.stringify(data), function (res) {
                truckmanager.list();
            }, function (err) {
                console.error("[save-add]", err);
            });
        }
    },
    edit: {
        open: function (id) {
            const divid = "truck-edit";

            truckmanager.ajax("GET", truckmanagerbaseurl + id, null, function (res) {
                if (!res.status) {
                    console.error(res.errorMsg);
                    return;
                }
                $(`#${divid}-id`).val(id);
                $(`#${divid}-name`).val(res.data.name);
                $(`#${divid}-type`).val(res.data.modelType);
                $(`#${divid}-manufactureyear`).val(res.data.manufactureYear);
                $(`#${divid}-modelyear`).val(res.data.modelYear);

                truckmanager.showdiv(divid);
            }, function (err) {
                console.error("[get-edit]", err);
            });
        },
        save: function () {
            const divid = "truck-edit";

            var data = {
                id: Number($(`#${divid}-id`).val()),
                name: $(`#${divid}-name`).val(),
                modelType: Number($(`#${divid}-type`).val()),
                manufactureYear: Number($(`#${divid}-manufactureyear`).val()),
                modelYear: Number($(`#${divid}-modelyear`).val()),
            };
            console.log(data);

            truckmanager.ajax("PUT", truckmanagerbaseurl, JSON.stringify(data), function (res) {
                truckmanager.list();
            }, function (err) {
                console.error("[save-edit]", err);
            });
        }
    },
    delete: function (id) {
        truckmanager.ajax("DELETE", truckmanagerbaseurl + id, null, function (res) {
            truckmanager.list();
        }, function (errr) {
            console.error("[save-delete]", err);
        });
    }
}