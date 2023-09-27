const countryUrl = 'Country', regionUrl = 'Region';
const countryTBody = document.getElementById('countriesContent').tBodies.item(0);

const countryInput = document.getElementById('countryForm'),
    countrySubmitBtn = document.getElementById('countryFormBtn'),
    countryError = document.getElementById('countryFormError');

// Country submit func
countrySubmitBtn.addEventListener('click', () => create(
    countryUrl + '/Upsert',
    JSON.stringify({ Name: countryInput.value.trim() }),
    countryError,
    instance => {
        insertParentRow(instance.id, instance.name, countryTBody, countryUrl);
        countryInput.value = '';
    }
));

for (const row of countryTBody.querySelectorAll('tr[data-entity-id]'))
    addParentRowFuncitionalities(row, countryUrl);


/**
 * Fetches child data & appends it to appropriate container
 * @param {HTMLTableElement} childrenContainerRow
 * @param {string} childFetchUrl
 */
function populateChildList(childrenContainerRow, childFetchUrl) {
    sendRequest({
        method: 'get',
        url: childFetchUrl,
        successFunction: children => {
            const childTbl = document.createElement('table');
            // TODO make css file better for baby tbl
            const tblBody = childTbl.createTBody();

            const tblHead = childTbl.createTHead();
            tblHead.className = 'd-none';

            // Adds header titles
            const headRow = tblHead.insertRow();
            headRow.insertCell().textContent = 'Action';
            headRow.insertCell().textContent = 'Region';

            // Displays children
            if (children.length == 0)
                childrenContainerRow.firstElementChild.textContent = 'No data';
            else {
                Array.from(children).forEach(region => sendRequest({
                    method: 'post',
                    url: `/ViewComponent/CreateCmsRow?id=${region.Id}&name=${region.Name}`,
                    successFunction: row => insertContentRow(row, tblBody, regionUrl, { CountryId: childrenContainerRow.dataset.parentId })
                }));
            }

            childrenContainerRow.firstElementChild.appendChild(childTbl);
        },
        failureFunction: () => childrenContainerRow.firstElementChild.innerHTML = '<span class="text-danger">Something went wrong: Try again later</span>'
    })
}

/**
 * 
 * @param {HTMLTableElement} childrenContainerRow   - row to contain/show all child data
 * @param {Element} dropdownIcon
 */
function addChildDropDownFunctionality(childrenContainerRow, dropdownIcon) {
    if (childrenContainerRow.style.display == 'none') {
        // Rotates icon & display children
        dropdownIcon.style.transform = 'rotate(90deg)';
        childrenContainerRow.style.display = 'table-row';
    } else {
        // Sets icon back to original place & hides children
        dropdownIcon.style.transform = 'rotate(-90deg)';
        childrenContainerRow.style.display = 'none';
    }
}

/**
 * Adds delete, update, and child functionalities to instance content row
 * @param {Element} row        - table row initialized with entity instance
 * @param {string} baseUrl     - url for delete (route defaulted to 'Delete/id') & edit (route defaulted to 'Upsert')
 */
function addParentRowFuncitionalities(row, baseUrl) {
    const instanceId = row.dataset.entityId;
    const regionsContainer = countryTBody.querySelector(`tr[data-parent-id='${instanceId}']`);

    // Sets update functionality
    row.children.item(1).addEventListener('keydown', e => keyPressUpdate(
        e,
        baseUrl + '/Upsert',
        JSON.stringify({ Id: instanceId, Name: e.target.textContent.trim() }),
        instanceId,
        row
    ));

    // Sets region list dropdown functionality
    row.firstElementChild.firstElementChild.addEventListener('click', e => {
        // Checks if children container has been opened (ergo, filled with child data) before
        // If not, populate it
        if (regionsContainer.firstElementChild.childElementCount == 0)
            populateChildList(regionsContainer, baseUrl + `/GetRegions/${instanceId}`);

        addChildDropDownFunctionality(regionsContainer, e.target);
    })

    // Region submit func
    row.lastElementChild.querySelector('.btn-primary').addEventListener('click', () => create(
        regionUrl + '/Upsert',
        JSON.stringify({ Name: document.getElementById(`regionForm-${instanceId}`).value, CountryId: instanceId }),
        document.getElementById(`regionFormError-${instanceId}`),
        success => {
            // Checks if children container has data, if so add new instance row
            // Otherwise it will be fetched with the rest when table is first displayed
            if (regionsContainer.firstElementChild.childElementCount > 0) {
                sendRequest({
                    method: 'post',
                    url: `/ViewComponent/CreateCmsRow?id=${success.id}&name=${success.name}`,
                    successFunction: childRow => insertContentRow(childRow, regionsContainer.querySelector('tbody'), regionUrl, { CountryId: instanceId })
                })
            }            
        }
    ));

    // Sets delete functionality
    row.lastElementChild.querySelector('.btn-danger').addEventListener('click', e => remove(
        countryUrl + `/Delete/${instanceId}`,
        () => row.remove())
    );
}

/**
 * Creates row (& child row), adds functionalities, adds it to top of table
 * @param {HTMLTableElement} tbody
 * @param {string} baseUrl      - url for delete (route defaulted to 'Delete/id') & edit (route defaulted to 'Upsert')
 */
function insertParentRow(instanceId, instanceName, tbody, baseUrl) {
    // Creates element from code string
    const template = document.createElement('template');

    // Add child row
    template.innerHTML = `<tr data-parent-id="${instanceId}" class="table-secondary" style="display: none;"><td colspan="3"></td></tr>`;
    tbody.insertBefore(template.content.firstElementChild, tbody.firstChild);

    // Add parent row
    template.innerHTML =
        `<tr data-entity-id="${instanceId}" class="border-bottom">` +
            '<td><button type="button" class="btn" title="Click to see regions"><i class="fa-solid fa-angle-right"></i></button></td>' +

            `<td><p contenteditable="true">${instanceName}</p>` +
                `<span class="text-danger" data-entity-id="${instanceId}" style="display: none;"></span></td>` +

            '<td class="d-flex">' +
                '<div><div class="input-group">' +
                    `<input type="text" class="form-control" id="regionForm-${instanceId}" placeholder="Region name" aria-label="Region name" aria-describedby="regionFormBtn-${instanceId}">` +
                    `<button class="btn btn-primary" type="button" id="regionFormBtn-${instanceId}">Add region</button>` +
                    '</div>' +
                    `<span id="regionFormError-${instanceId}" class="text-danger" style="display: none;"></span></div>` +

                '<button type="button" class="btn btn-danger">Delete</button>' +
            '</td>' +
        '</tr>';

    const parentRowDOM = template.content.firstElementChild;

    addParentRowFuncitionalities(parentRowDOM, baseUrl);  // Adds delete & edit functionalities
    tbody.insertBefore(parentRowDOM, tbody.firstChild);   // Adds row to table
}
