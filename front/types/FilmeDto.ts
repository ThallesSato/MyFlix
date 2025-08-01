import {Filme} from "@/types/Filme";

export type FilmeDto = Omit<Filme, 'id' | 'status' | 'nota'>