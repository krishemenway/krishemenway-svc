import { Observable } from "@residualeffect/reactor";
import {default as PunchOutBaldBull} from "Arts/Images/PunchOut-BaldBull.png";
import {default as PunchOutDonFlamenco} from "Arts/Images/PunchOut-DonFlamenco.png";
import {default as PunchOutGlassJoe} from "Arts/Images/PunchOut-GlassJoe.png";
import {default as PunchOutGreatTiger} from "Arts/Images/PunchOut-GreatTiger.png";
import {default as PunchOutKingHippo} from "Arts/Images/PunchOut-KingHippo.png";
import {default as PunchOutPistonHonda} from "Arts/Images/PunchOut-PistonHonda.png";
import {default as PunchOutSodaPopinski} from "Arts/Images/PunchOut-SodaPopinski.png";
import {default as PunchOutVonKaiser} from "Arts/Images/PunchOut-VonKaiser.png";
import {default as TMNT} from "Arts/Images/TMNT.png";
import {default as Airbender} from "Arts/Images/Airbender.png";

export interface ArtMetadata {
	Title: string;
	FullPath: string;
	ThumbnailPath: string;
}

export class ArtService {
	constructor() {
		this.SelectedArtMetadata = new Observable<ArtMetadata|null>(null);
	}

	public SelectedArtMetadata: Observable<ArtMetadata|null>;

	public AllArtByCategory : Dictionary<ArtMetadata[]> = {
		"Punch Out": [
			{ Title: "Bald Bull", FullPath: PunchOutBaldBull, ThumbnailPath: PunchOutBaldBull },
			{ Title: "Don Flamenco", FullPath: PunchOutDonFlamenco, ThumbnailPath: PunchOutDonFlamenco },
			{ Title: "Glass Joe", FullPath: PunchOutGlassJoe, ThumbnailPath: PunchOutGlassJoe },
			{ Title: "Great Tiger", FullPath: PunchOutGreatTiger, ThumbnailPath: PunchOutGreatTiger },
			{ Title: "King Hippo", FullPath: PunchOutKingHippo, ThumbnailPath: PunchOutKingHippo },
			{ Title: "Piston Honda", FullPath: PunchOutPistonHonda, ThumbnailPath: PunchOutPistonHonda },
			{ Title: "Soda Popinski", FullPath: PunchOutSodaPopinski, ThumbnailPath: PunchOutSodaPopinski },
			{ Title: "Von Kaiser", FullPath: PunchOutVonKaiser, ThumbnailPath: PunchOutVonKaiser },
		],
		"TMNT": [
			{ Title: "Moods", FullPath: TMNT, ThumbnailPath: TMNT },
		],
		"Futurama": [
			{ Title: "Airbender", FullPath: Airbender, ThumbnailPath: Airbender },
		],
	};

	public static Instance: ArtService = new ArtService();
}
