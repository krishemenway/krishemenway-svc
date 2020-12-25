export function BuildQueryString() {
	return window.location.search
		.slice(1).split("&")
		.map((keyValuePairString) => keyValuePairString.split("="))
		.reduce((queryStringDictionary, current) => { queryStringDictionary[current[0]] = current[1]; return queryStringDictionary; }, {} as Dictionary<string>);
}
