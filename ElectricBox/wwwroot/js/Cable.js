let typeCalcForTypeCurrent = "P";//нужна для того чтобы загрузить поле ввода коэф мощности при переключении с DC на AC 
let typeCurrentForTypeCalc = "AC";

function calc() {
    //получаем все необходимые данные для расчетов
    let material = document.getElementById('material').options[document.getElementById('material').selectedIndex].text;
    let cableLenght = parseFloat((document.getElementById('cableLenght').value).replace(",", "."));
    let typeCurrent;
    let typeCurrents = document.getElementsByName('typeCurrent');
    for (let i = 0; i < typeCurrents.length; i++) {
        if (typeCurrents[i].checked) {
            typeCurrent = typeCurrents[i].value;
            break;
        }
    }
    let network;
    let networks;
    if (typeCurrent == "AC") {
        network;
        networks = document.getElementsByName('network');
        for (let i = 0; i < networks.length; i++) {
            if (networks[i].checked) {
                network = networks[i].value;
                break;
            }
        }
    } else if (typeCurrent == "DC") {
        network == "1F";
    }
    let voltage = parseFloat((document.getElementById('voltage').value).replace(",", "."));
    let deltaVoltage = parseFloat((document.getElementById('deltaVoltage').value).replace(",", "."));
    let typeCalc;
    let types = document.getElementsByName('typeCalc');
    for (let i = 0; i < types.length; i++) {
        if (types[i].checked) {
            typeCalc = types[i].value;
            break;
        }
    }
    let power;
    let cosPhi;
    let current;
    if (typeCalc == "P") {
        power = parseFloat((document.getElementById('power').value).replace(",", "."));
        if (typeCurrent == "AC") {
            cosPhi = parseFloat((document.getElementById('cosPhi').value).replace(",", "."));
        }
    } else if (typeCalc == "I") {
        current = parseFloat((document.getElementById('current').value).replace(",", "."));
    }
    let unit;
    for (let i = 0; i < unitResistance.length; i++) {
        if (unitResistance[i].material == material) {
            unit = parseFloat(unitResistance[i].unitResistance.replace(",", "."));
            break;
        }
    }
    let method = document.getElementById('typePad').options[document.getElementById('typePad').selectedIndex].text;
    let I;//расчетный ток
    let S;//расчетное сечение

    //прверка правильности введенных данных
    let correct = true;
    if (material == "Выберите материал проводника" || cableLenght == NaN || voltage == NaN || deltaVoltage == NaN || method == "Выберите способ прокладки") {
        correct = false;
    }
    if (correct && typeCurrent == "DC") {
        if (typeCalc == "P" && power == NaN) {
            correct = false;
        }
        if (typeCalc == "I" && current == NaN) {
            correct = false;
        }
    }
    if (correct && typeCurrent == "AC") {
        if (typeCalc == "P" && (power == NaN || cosPhi == NaN)) {
            correct = false;
        }
        if (typeCalc == "I" && current == NaN) {
            correct = false;
        }
    }

    //расчеты
    if (typeCurrent == "AC") {
        if (typeCalc == "P") {
            I = (power) / (voltage * cosPhi);

            if (network == "3F") {
                I = I / Math.sqrt(3);
            }

            S = (2 * unit * cableLenght * I * 100) / (voltage * deltaVoltage);

        } else if (typeCalc == "I") {
            S = (2 * unit * cableLenght * current * 100) / (voltage * deltaVoltage);
        }
    } else if (typeCurrent == "DC") {
        if (typeCalc == "P") {
            I = power / voltage;
            S = (2 * unit * cableLenght * I * 100) / (voltage * deltaVoltage);
        } else if (typeCalc == "I") {
            S = (2 * unit * cableLenght * current * 100) / (voltage * deltaVoltage);
        }
    }

    //уточняем расчетные данные
    if (material == "Медь" && typeCalc == "P") {
        S = clarifyCut(cuprumCurrents, method, I, S);
    } else if (material == "Алюминий" && typeCalc == "P") {
        S = clarifyCut(aluminiumCurrents, method, I, S);
    } else if (material == "Медь" && typeCalc == "I") {
        S = clarifyCut(cuprumCurrents, method, current, S);
    } else if (material == "Алюминий" && typeCalc == "I") {
        S = clarifyCut(aluminiumCurrents, method, current, S);
    }

    //вывод расчетных данных и проверка корректности введенных данных
    if (cosPhi <= 0 || cosPhi > 1) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p style="color: red;"><b>Коэффициент мощности не может быть больше 1 или меньше 0!!!</b></p>`);
    } else if (deltaVoltage < 0 || deltaVoltage > 100) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p style="color: red;"><b>Потери напряжения должны лежать в диапазоне 0-100%!!!</b></p>`);
    } else if (voltage <= 0 || (power < 0 && typeCalc == "P") || (current < 0 && typeCalc == "I") || cableLenght <= 0) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p style="color: red;"><b>Ведены не корректные данные!!!</b></p>`);
    } else if (correct) {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p>Минимальное требуемое значение проводника - <b>${S.toFixed(1)} мм<sup>2</sup></b></p>`);

        if (material == "Медь") {
            loadTable(cuprumCurrents, S);
        } else if (material == "Алюминий") {
            loadTable(aluminiumCurrents, S);
        } else {
            $("#answear").append(`<p><b><i><u>!!!Подумайте над тем стоит ли выбирать такой проводник!!!</u></i></b></p>`);
        }

    } else {
        $("#answear").remove();
        $("#partRight").append(`<div id='answear'></div>`);
        $("#answear").append(`<p style="color: red;"><b>Не достаточно данных для расчета!!!</b></p>`);
    }
}

function loadTable(array, S) {//загрузка даблици Допустимых длительных токов 
    $("#answear").append(`<table id="table" >
                                    <tr>
                                        <th></th>
                                        <th colspan="6">Ток, А, для проводов, проложенных в одной трубе</th>
                                    </tr>        
                                  </table>`);
    $("#table").append(`<tr id="head"><th>Сечение токопроводящей жилы, мм<sup>2</sup></th></tr>`);
    for (let i = 0; i < methods.length; i++) {
        $("#head").append(`<th>${methods[i].method}</th>`)
    }

    let startIndex;
    for (let i = 0; i < cuts.length; i++) {
        if (parseFloat(cuts[i].cut.replace(",", ".")) == parseFloat(array[0].cut.replace(",", "."))) {
            startIndex = i;
        }
    }

    for (let i = startIndex; i < cuts.length; i++) {
        $("#table").append(`<tr id="row${i}"><td id="current${i}">${cuts[i].cut}</td></tr>`);

        if (parseFloat(cuts[i].cut.replace(",", ".")) == S) {
            document.getElementById(`current${i}`).setAttribute("style", "background-color:lightgreen;");            
        }

        for (let k = 0; k < methods.length; k++) {
            let havesmth = false;
            for (let j = 0; j < array.length; j++) {  
                if (parseFloat(cuts[i].cut.replace(",", ".")) == parseFloat(array[j].cut.replace(",", ".")) && methods[k].method.toString() == array[j].method.toString()) {
                    $(`#row${i}`).append(`<td>${array[j].current}</td>`);
                    havesmth = true;
                    break;
                }                
            }
            if (!havesmth) {
                $(`#row${i}`).append(`<td>-</td>`);
            }
        }
    }
}

function clarifyCut(array, method, _I, _S) {//Утончяем сечение проводника
    let clarify_S = _S;

    for (let i = 0; i < array.length; i++) {
        if (array[i].method == method && _I <= parseFloat(array[i].current.replace(",", "."))) {
            let S_tmp = parseFloat(array[i].cut.replace(",", "."));

            if (S_tmp < _S) {
                for (let j = 0; j < cuts.length; j++) {
                    let _cut_float = parseFloat(cuts[j].cut.replace(",", "."));

                    if (_S <= _cut_float) {
                        clarify_S = _cut_float;
                        break;
                    }
                }
                break;
            } else {
                clarify_S = S_tmp;
                break;
            }
        }
    }
    return clarify_S;
}

function changeTypeCalc(radio) {//переключатель параметров расчета
    typeCalcForTypeCurrent = radio.value;

    if (radio.value == "P") {
        $("#type").remove();
        $("#typeCalc").append(`<div id='type'>
                                    <label>7. Мощность (Вт): </label>
                                    <input id='power' type="number" onfocus="getFocus(this)" onblur="lostFocus(this)"/>
                                    <div id="phi"></div>
                                </div>`);
        document.getElementById('power').setAttribute("style", "height: 25px; border - bottom - width: thin; width: 70px; text - align: right; -moz - appearance: textfield; -webkit-appearance: none; margin: 0;");

        if (typeCurrentForTypeCalc == "AC") {
            $("#phi").append(`
                    <label>8. Коэффициент мощности <i>cos &phi;</i> (&lt;1): </label>
                    <input id='cosPhi' type="number" onfocus="getFocus(this)" onblur="lostFocus(this)"/>`);
            document.getElementById('cosPhi').setAttribute("style", "height: 25px; border - bottom - width: thin; width: 70px; text - align: right; -moz - appearance: textfield; -webkit-appearance: none; margin: 0;");
        }
    } else if (radio.value == "I") {
        $("#type").remove();
        $("#typeCalc").append(`<div id='type'>
                                    <label>7. Ток (А): </label>
                                    <input id='current' type="number" onfocus="getFocus(this)" onblur="lostFocus(this)"/>
                               </div>`);
        document.getElementById('current').setAttribute("style", "height: 25px; border - bottom - width: thin; width: 70px; text - align: right; -moz - appearance: textfield; -webkit-appearance: none; margin: 0;");
    }
}

function changeTypeCurrent(radio) {//переключатель питающей сети
    typeCurrentForTypeCalc = radio.value;

    if (radio.value == "AC") {
        $("#networkType").append(`<label id="label_3_1">3.1. Питающая сеть:</label>`);
        $("#networkType").append(`<div id='network' class="radio_btn"></div>`);
        $("#network").append(`<input id="1F" type="radio" name="network" checked value="1F" />
                              <label for="1F">Однофазная</label>
                              <input id="3F" type="radio" name="network" value="3F" />
                              <label for="3F">Трехфазная</label>`);
        if (typeCalcForTypeCurrent == "P") {
            $("#type").append(`<div id="phi">
                                    <label>8. Коэффициент мощности <i>cos &phi;</i> (&lt;1): </label>
                                    <input id='cosPhi' type="number" onfocus="getFocus(this)" onblur="lostFocus(this)"/>
                               </div>`);
            document.getElementById('cosPhi').setAttribute("style", "height: 25px; border - bottom - width: thin; width: 70px; text - align: right; -moz - appearance: textfield; -webkit-appearance: none; margin: 0;");
        }
    } else if (radio.value == "DC") {
        $("#network").remove();
        $("#label_3_1").remove();
        $("#phi").remove();
        
    }
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

function choiseOption(select) {
    const elem = select.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "background-color: lightgreen;");
}

function optionGetFocus(select) {
    const elem = select.getAttribute("id");
    document.getElementById(elem).setAttribute("style", "background-color: white;");
}
