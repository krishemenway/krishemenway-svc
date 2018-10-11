import * as React from 'react';
import * as reactDom from 'react-dom';
import { Calendar } from "./Calendar";

export class App extends React.Component<{}, {}> {
	public render() {
		return (
			<Calendar />
		)
	}
}

reactDom.render(<App />, document.getElementById('app'));
