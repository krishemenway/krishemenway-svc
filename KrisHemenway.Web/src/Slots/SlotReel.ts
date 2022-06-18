import { Receiver } from "@krishemenway/react-loading-component";
import { Observable } from "@residualeffect/reactor";

export class SlotReel {
	constructor() {
		this.CurrentSymbol = new Receiver<string>("Failed to get symbol");
		this.IsLocked = new Observable(false);
	}

	public ToggleLock(): void {
		if (this.CurrentSymbol.IsBusy.Value) {
			return;
		}

		this.IsLocked.Value = !this.IsLocked.Value;
	}

	public CurrentSymbol: Receiver<string>;
	public IsLocked: Observable<boolean>;
}
