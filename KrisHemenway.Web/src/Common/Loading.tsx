import * as React from "react";

interface LoadingProps {
	IsLoading: boolean;
	Render: () => JSX.Element|JSX.Element[];
}

const Loading : React.FC<LoadingProps> = (props) => {
	if (props.IsLoading) {
		return <>Loading...</>;
	}

	return <>{props.Render()}</>;
};

export default Loading;