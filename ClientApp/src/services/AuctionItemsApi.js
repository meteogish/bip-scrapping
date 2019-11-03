export async function fetchAuctionItems(searchText, searchDate) {
    const params = {
      searchText: searchText,
      fromDate: searchDate.toISOString().substring(0, 10)
    };

    const response = await fetchWithParams('auctionItems', 
    {
      queryParams: params
    });

    return await response.json();
}

function fetchWithParams(url, options={}) {
    if(options.queryParams) {
        url += (url.indexOf('?') === -1 ? '?' : '&') + queryParams(options.queryParams);
        delete options.queryParams;
    }

    return fetch(url, options);
}

function queryParams(params) {
    return Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
}