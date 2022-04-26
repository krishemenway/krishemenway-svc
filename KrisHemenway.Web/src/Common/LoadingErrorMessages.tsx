import * as React from "react";
import clsx from "clsx";
import { useMargin, useText } from "Common/AppStyles";
import ListOf from "Common/ListOf";

const LoadingErrorMessages: React.FC<{ errorMessages: string[] }> = (props) => {
	const [text, margin] = [useText(), useMargin()];

	return (
		<ListOf
			items={props.errorMessages}
			createKey={(message) => message}
			renderItem={(message) => <>{message}</>}
			listClassName={clsx(text.center, margin.topDouble)}
			listItemClassName={() => clsx(margin.bottom)}
		/>
	);
};

export default LoadingErrorMessages;