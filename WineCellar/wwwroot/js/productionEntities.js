const varietalUrl = 'Varietal', producerUrl = 'WineProducer';

const varietalsContainer = document.getElementById('varietalsContent'), varietalTBody = varietalsContainer.querySelector('tbody');
const producersContainer = document.getElementById('producersContent'), producerTBody = producersContainer.querySelector('tbody');

// Form inputs & submit buttons
const varietalInput = document.getElementById('varietalForm'),
    varietalSubmitBtn = document.getElementById('varietalFormBtn'),
    varietalError = document.getElementById('varietalFormError');

const producerInput = document.getElementById('producerForm'),
    producerSubmitBtn = document.getElementById('producerFormBtn'),
    producerError = document.getElementById('varietalFormError');;

// Varietal submit func
varietalSubmitBtn.addEventListener('click', () => create(
    varietalUrl + '/Upsert',
    JSON.stringify({ Name: varietalInput.value.trim() }),
    varietalError,
    instance => sendRequest({
        method: 'post',
        url: `/ViewComponent/CreateCmsRow?id=${instance.id}&name=${instance.name}`,
        // Adds row to table with functionalities & clear form
        successFunction: row => {
            insertContentRow(row, varietalTBody, varietalUrl);
            varietalInput.value = '';
        },
        failureFunction: response => showStatusToast(response.message, false)
    })
));

// Wine producer submit func
producerSubmitBtn.addEventListener('click', () => create(
    producerUrl + '/Upsert',
    JSON.stringify({ Name: producerInput.value.trim() }),
    producerError,
    instance => sendRequest({
        method: 'post',
        url: `/ViewComponent/CreateCmsRow?id=${instance.id}&name=${instance.name}`,
        // Adds row to table & clear form
        successFunction: row => {
            insertContentRow(row, producerTBody, producerUrl);
            producerInput.value = '';
        },
        failureFunction: response => showStatusToast(response.message, false)
    })
));

for (const row of varietalTBody.children)
    addRowFunctionalities(varietalTBody, row, varietalUrl);

for (const row of producerTBody.children)
    addRowFunctionalities(producerTBody, row, producerUrl);