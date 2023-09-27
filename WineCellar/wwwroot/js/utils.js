// Toast bg colors
const successColor = '#198754', failureColor = '#ff3333';

/**
 * 
 * @param {string} message
 * @param {string} isSuccessful - HEX code string
 */
function showStatusToast(message, isSuccessful) {
    Toastify({ text: message, close: true, style: { background: isSuccessful ? successColor : failureColor } }).showToast();
}

/**
 * Adds delete & update functionalities to instance content row
 * @param {HTMLTableSectionElement} tbody
 * @param {Element} row             - Table row initialized with entity instance
 * @param {string} baseUrl          - Url for delete (route defaulted to 'Delete/id') & edit (route defaulted to 'Upsert')
 * @param {Object} additionalData   - Data (aside entity instance id & name) to also be sent in update request
 */
function addRowFunctionalities(tbody, row, baseUrl, additionalData = null) {
    // Delete functionality
    row.querySelector('button').addEventListener('click', () => remove(
        baseUrl + `/Delete/${row.dataset.entityId}`,
        () => tbody.querySelector(`[data-entity-id='${row.dataset.entityId}']`).remove()    // Remove instance row
    ))
    
    // Edit functionality
    row.querySelector('[contenteditable=true]').addEventListener('keydown', e => keyPressUpdate(
        e,
        baseUrl + '/Upsert',
        JSON.stringify(Object.assign({ Id: Number(e.target.dataset.entityId), Name: e.target.textContent.trim() }, additionalData)),
        e.target.dataset.entityId,
        row
    ));
}

/**
 * Creates row, adds functionalities, adds it to top of table
 * @param {string} rowString    
 * @param {HTMLTableSectionElement} tbody
 * @param {string} baseUrl      - url for delete (route defaulted to 'Delete/id') & edit (route defaulted to 'Upsert')
 * @param {Object} additionalData   - Data (aside entity instance id & name) to also be sent in update request
 */
function insertContentRow(rowString, tbody, baseUrl, additionalData = null) {
    // Creates element from code string
    const template = document.createElement('template');
    template.innerHTML = rowString;
    const rowDOM = template.content.firstElementChild;

    addRowFunctionalities(tbody, rowDOM, baseUrl, additionalData);  // Adds delete & edit functionalities
    tbody.insertBefore(rowDOM, tbody.firstChild);   // Adds row to table
}

// CRUD

/**
 * Sends post request, adds element in view & shows success toast or show failure toast
 * @param {string} url
 * @param {string} data             - JSON string
 * @param {Element} errorElement    - element that displays error message(s)
 * @param {Function} addDOMFunction - function to add created element in view
 */
function create(url, data, errorElement, addDOMFunction) {
    sendRequest({
        method: 'post',
        url: url,
        successCode: 201,
        data: data,
        successFunction: response => {
            errorElement.textContent = '';
            errorElement.style.display = 'none';

            addDOMFunction(addDOMFunction.length ? response : null);
            showStatusToast(`${response.name} was successfully added`, true);
        },
        failureFunction: response => {
            errorElement.textContent = response.message;
            errorElement.style.display = 'block';
        }
    });
}

/**
 * Sends post request when 'enter' key is pressed
 * @param {Event} event
 * @param {string} url
 * @param {Number} instanceId
 * @param {string} data            - JSON string
 * @param {Element} container   - container holding instance content
 */
function keyPressUpdate(event, url, data, instanceId, container) {
    if (event.code === 'Enter') {
        event.preventDefault();
        update(url, data, instanceId, container);
    }
}

/**
 * Sends post request, shows error message if failed or shows success toast if successful
 * @param {string} url
 * @param {Number} instanceId
 * @param {string} data         - JSON string
 * @param {Element} container   - container holding instance content
 */
function update(url, data, instanceId, container) {
    const txtError = container.querySelector(`.text-danger[data-entity-id='${instanceId}']`);

    sendRequest({
        method: 'post',
        url: url,
        data: data,
        successFunction: () => {
            txtError.textContent = '';
            txtError.style.display = 'none';

            showStatusToast('Update successful', true);
        },
        failureFunction: response => {
            txtError.textContent = response.message;
            txtError.style.display = 'block';
        }
    });
}

/**
 * Sends delete request, removes item in view & returns success/failure message
 * @param {string} url                  - delete url
 * @param {Function} removeDOMFunction  - function to remove item from view
 */
function remove(url, removeDOMFunction) {
    sendRequest({
        method: 'delete',
        url: url,
        successCode: 204,
        successFunction: () => {
            removeDOMFunction();
            showStatusToast("Deletion successful", true);
        },
        failureFunction: response => showStatusToast(response.message, false)
    });
}

/**
 * Facilitates sending ajax request
 * @param {Object} request
 * @param {string} request.method
 * @param {string} request.url
 * @param {string} request.contentType
 * @param {Number} request.successCode          - HTTP status code
 * @param {string} request.data                 - JSON string
 * @param {Function} request.successFunction
 * @param {Function} request.failureFunction
 */
function sendRequest({ method, url, contentType = 'application/json', successCode = 200, data = null, successFunction = null, failureFunction = null }) {
    const request = new XMLHttpRequest();
    request.open(method, url);

    request.setRequestHeader("Content-Type", contentType);

    request.onload = function () {
        var response;

        // returns as JSON if parsable
        try {
            response = JSON.parse(this.response);
        } catch (e) {
            response = this.response;
        }

        if (this.status === successCode) {
            if (successFunction) successFunction(successFunction.length != 0 ? response : null);
        }
        else {
            if (failureFunction) failureFunction(failureFunction.length != 0 ? response : null);
        }
    };

    request.send(data);
}