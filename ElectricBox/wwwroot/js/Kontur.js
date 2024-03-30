    
function calc() {
    //получаем все необходимые данные для расчетов
    const typeVerEl = document.getElementById('typeVerEl').options[document.getElementById('typeVerEl').selectedIndex].text;
    const vertElSechenie = parseFloat((document.getElementById('vertElSechenie').value).replace(",", "."));
    const vertElCount = parseInt(document.getElementById('vertElCount').value);
    const vertElLenght = parseFloat((document.getElementById('vertElLenght').value).replace(",", "."));
    const vertElLenghtBeetwean = parseFloat((document.getElementById('vertElLenghtBeetwean').value).replace(",", "."));
    const typeHorEl = document.getElementById('typeHorEl').options[document.getElementById('typeHorEl').selectedIndex].text;
    const horElSechenie = parseFloat((document.getElementById('horElSechenie').value).replace(",", "."));
    const horElLenght = parseFloat((document.getElementById('horElLenght').value).replace(",", "."));
    const horElH = parseFloat((document.getElementById('horElH').value).replace(",", "."));
    const ground = document.getElementById('ground').options[document.getElementById('ground').selectedIndex].text;
    const zona = document.getElementById('zona').options[document.getElementById('zona').selectedIndex].text;
    let schema;
    const schemas = document.getElementsByName('schema');
    for (let i = 0; i < schemas.length; i++) {
        if (schemas[i].checked) {
            schema = schemas[i].value;
            break;
        }
    }
    const pi = 3.1415926;

    //расчет сопротивления одного вертикального электрода:

    //приведем сечение верт электрода к диаметру
    let d = Math.sqrt(4 * vertElSechenie / pi) / 1000;//в метрах

    //найдем глубину заложения вертикального электрода
    let t = (vertElLenght + horElH) / 2;

    //удельное сопротивление грунта
    let p;
    for (let i = 0; i < groundResistance.length; i++) {
        if (groundResistance[i].ground == ground) {
            p = parseFloat(groundResistance[i].resistance.replace(",", "."));
            break;
        }
    }

    //поправочный коэффициент климатической зоны
    let psi_vert;
    let psi_hor;
    for (let i = 0; i < climaticZoneFactors.length; i++) {
        if (climaticZoneFactors[i].zona == zona) {
            psi_vert = parseFloat(climaticZoneFactors[i].climaticFactorVert.replace(",", "."));
            psi_hor = parseFloat(climaticZoneFactors[i].climaticFactorHor.replace(",", "."));
            break;
        }
    }

    //сопротивление растеканию одного верт электрода =
    let R_1el = (p / (2 * pi * vertElLenght)) * (Math.log(2 * vertElLenght / d) + (Math.log((4 * t + vertElLenght) / (4 * t - vertElLenght))) / 2);

    //общее сопротивление вертикальных электродов без учета горизонтального
    let R_vert;
    let factorUseVert;
    let ratio = vertElLenghtBeetwean / vertElLenght;
    if (ratio < 0.5) ratio = 1;
    for (let i = 0; i < factorsUseVerticalElectrodes.length; i++) {
        if (vertElCount <= parseInt(factorsUseVerticalElectrodes[i].countEl) && Math.round(ratio) == parseInt(factorsUseVerticalElectrodes[i].ratio)) {
            if (i == 0 || i == 7 || i == 14) {
                factorUseVert = parseFloat(factorsUseVerticalElectrodes[i].factorUse.replace(",", "."));
                break;
            } else {
                factorUseVert = parseFloat(factorsUseVerticalElectrodes[i].factorUse.replace(",", ".")) -
                    ((parseInt(factorsUseVerticalElectrodes[i].countEl) - vertElCount) *
                        (parseFloat(factorsUseVerticalElectrodes[i].factorUse.replace(",", ".")) - parseFloat(factorsUseVerticalElectrodes[i - 1].factorUse.replace(",", "."))) /
                        (parseInt(factorsUseVerticalElectrodes[i].countEl) - parseInt(factorsUseVerticalElectrodes[i - 1].countEl)));
                break;
            }
        } else if (vertElCount > parseInt(factorsUseVerticalElectrodes[6].countEl)) {
            if (Math.round(ratio) == parseInt(factorsUseVerticalElectrodes[i].ratio)) {
                factorUseVert = (parseInt(factorsUseVerticalElectrodes[i].countEl) / vertElCount) * parseFloat(factorsUseVerticalElectrodes[i].factorUse.replace(",", "."));
                break;
            }
        }
    }
    R_vert = (R_1el * psi_vert) / (vertElCount * factorUseVert);

    //определяем сопротиввление растеканию горизонтального электрода
    let R_hor;

    //выбираем коэф исп горизонтального электрода
    let factorUseHor;
    for (let i = 0; i < factorsUseHorizontalElectrodes.length; i++) {
        if (vertElCount <= parseInt(factorsUseHorizontalElectrodes[i].countEl) && Math.round(ratio) == parseInt(factorsUseHorizontalElectrodes[i].ratio)) {
            if (i == 0 || i == 9 || i == 18) {
                factorUseHor = parseFloat(factorsUseHorizontalElectrodes[i].factorUse.replace(",", "."));
                break;
            } else {
                factorUseHor = parseFloat(factorsUseHorizontalElectrodes[i].factorUse.replace(",", ".")) -
                    ((parseInt(factorsUseHorizontalElectrodes[i].countEl) - vertElCount) *
                        (parseFloat(factorsUseHorizontalElectrodes[i].factorUse.replace(",", ".")) - parseFloat(factorsUseHorizontalElectrodes[i - 1].factorUse.replace(",", "."))) /
                        (parseInt(factorsUseHorizontalElectrodes[i].countEl) - parseInt(factorsUseHorizontalElectrodes[i - 1].countEl)));
                break;
            }
        } else if (vertElCount > parseInt(factorsUseHorizontalElectrodes[8].countEl)) {
            if (Math.round(ratio) == parseInt(factorsUseHorizontalElectrodes[i].ratio)) {
                factorUseHor = (parseInt(factorsUseHorizontalElectrodes[i].countEl) / vertElCount) * parseFloat(factorsUseHorizontalElectrodes[i].factorUse.replace(",", "."));
                break;
            }
        }
    }

    let b;//ширина полосы
    if (typeHorEl == "Полоса") {
        b = horElSechenie / 5 / 1000;
    } else {
        b = 2 * Math.sqrt(4 * horElSechenie / pi) / 1000;
    }

    //в зависимости от схемы контура вычисляем сопротивление растеканию горизонтального элетрода
    switch (schema) {
        case 'line': {//если заземлитель разомкнутый
            R_hor = ((p * psi_hor / factorUseHor) / (2 * pi * horElLenght)) * Math.log(2 * Math.pow(horElLenght, 2) / (b * t));
            break;
        }
        case 'circle': {//если заземлитель замкнутый
            R_hor = ((p * psi_hor / factorUseHor) / (2 * pi * pi * horElLenght)) * Math.log((8 * horElLenght * horElLenght) / (b * t));
            break;
        }
        default: break;
    }

    //Общее сопротивления контура растеканию тока
    let R = (R_vert * R_hor) / (R_vert + R_hor);
    
    if (vertElCount == 1 && (typeVerEl != "Выберите профиль электродов" && vertElSechenie != NaN && vertElCount != NaN && vertElLenght != NaN && horElH != NaN &&
        ground != "Выберите тип грунта" && zona != "Выберите климатическую зону")) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $('#answear').append(`<img src='/R0.png' class="calculationsPic" />`);
        $("#answear").append(`<p><b>ответ: <i>${R_1el.toFixed(2)}</i></b></p>`);
    } else if (vertElCount > 1 && (typeVerEl != "Выберите профиль электродов" && vertElSechenie != NaN && vertElCount != NaN && vertElLenght != NaN &&
        vertElLenghtBeetwean != NaN && typeHorEl != "Выберите профиль сечения" && horElSechenie != NaN && horElLenght != NaN && horElH != NaN &&
        ground != "Выберите тип грунта" && zona != "Выберите климатическую зону")) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p><b>- сопротивление 1-го вертикального электрода:</b></p>`);
        $('#answear').append(`<img src='/R0.png' class="calculationsPic"/>`);
        $("#answear").append(`<p><b>- сопротивление горизонтального электрода:</b></p>`);
        if (schema == 'line') {
            $('#answear').append(`<img src='/R_hor_line.png' class="calculationsPic"/>`);
        } else {
            $('#answear').append(`<img src='/R_hor_circle.png' class="calculationsPic"/>`);
        }
        $("#answear").append(`<p><b>– общее сопротивление контура заземления растеканию тока:</b></p>`);
        $('#answear').append(`<img src='/R.png' class="calculationsPic"/>`);
        $("#answear").append(`<p><b>ответ: <i>${R.toFixed(2)}</i></b></p>`);
    } else {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear' style="color: red;"><p><b>Недостаточно данных для расчета!\nПожалуйста заполните все необходимые данные!</b></p></div>`);
    }
}

function choiseOption(select) {
    const elem = select.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "background-color: lightgreen;");
}

function optionGetFocus(select) {
    const elem = select.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "background-color: white;");
}

function getFocus(input) {
    const elem = input.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "background-color: lightsteelblue;");
}

function lostFocus(input) {
    const elem = input.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "border-bottom-width: thin;");
    const value = document.getElementById(elem).value;
    if (value != "") {
        document.getElementById(elem).setAttribute("style", "background-color: lightgreen;");
    }
}
