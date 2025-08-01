import axios from 'axios';
import { Filme } from '@/types/Filme';
import { FilmeDto } from '@/types/FilmeDto';

const api = axios.create({
    baseURL: 'http://localhost:5096/'
});

export const filmeService = {
    getAll: async (): Promise<Filme[]> => {
        const response = await api.get('filme');
        return response.data;
    },

    update: async (id: number, filme: FilmeDto): Promise<void> => {
        const response = await api.put(`filme/${id}`, filme);
        return response.data;
    },

    delete: async (id: number): Promise<void> => {
        await api.delete(`filme/${id}`);
    },

    post: async (filmeDto: FilmeDto): Promise<void> => {
        await api.post(`filme`, filmeDto);
    },

    rateFilme: async (id: number, nota: number): Promise<void> => {
        await api.put(`filme/nota/${id}`, nota, {
            headers: { "Content-Type": "application/json" }
        });
    }

};