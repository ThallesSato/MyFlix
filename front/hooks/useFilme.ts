'use client'

import { useState, useEffect } from "react";
import { FilmeDto } from "@/types/FilmeDto";
import { Filme } from "@/types/Filme";
import { filmeService } from "@/services/filmeService";

export function useFilme(showAlert: (message: string, type: "success" | "error") => void) {
	const [filmes, setFilmes] = useState<Filme[]>([]);

	const loadFilmes = async () => {
		try {
			const data = await filmeService.getAll();
			setFilmes(data);
		} catch (error) {
			console.error("Erro ao carregar filmes:", error);
			showAlert("Erro ao carregar filmes", "error");
		} finally {
		}
	};

	const addFilme = async (filmeDto: FilmeDto) => {
		try {
			await filmeService.post(filmeDto);
			showAlert("Filme adicionado com sucesso!", "success");
			await loadFilmes();
		} catch (error)  {
			console.error("Erro ao adicionar filme:", error);
			showAlert("Erro ao adicionar filme", "error");
		}
	};

	const updateFilme = async (id: number, filmeDto: FilmeDto) => {
		try {
			await filmeService.update(id, filmeDto);
			showAlert("Filme atualizado com sucesso!", "success");
			await loadFilmes();
		} catch (error) {
			console.error("Erro ao atualizar filme:", error);
			showAlert("Erro ao atualizar filme", "error");
		}
	};

	const deleteFilme = async (id: number) => {
		try {
			await filmeService.delete(id);
			showAlert("Filme excluído com sucesso!", "success");
			await loadFilmes();
		} catch (error) {
			console.error("Erro ao excluir filme:", error);
			showAlert("Erro ao excluir filme", "error");
		}
	};

	const rateFilme = async (id: number, rating: number) => {
		try {
			await filmeService.rateFilme(id, rating);
			showAlert("Avaliação salva com sucesso!", "success");
			await loadFilmes();
		} catch (error) {
			console.error("Erro ao avaliar filme:", error);
			showAlert("Erro ao avaliar filme", "error");
		}
	};

	useEffect(() => {
		loadFilmes();
	}, []);

	return { filmes, addFilme, updateFilme, deleteFilme, rateFilme };
}
